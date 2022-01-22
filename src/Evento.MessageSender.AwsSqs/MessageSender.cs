using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudEventData;
using Evento.Api.Contracts;
using Microsoft.Extensions.Logging;

namespace Evento.MessageSender.AwsSqs
{
    public class MessageSender : IMessageSender
    {
        private readonly IBusSettings _messageBusSettings;
        private readonly ILogger _logger;

        public MessageSender(IBusSettings messageBusSettings, ILogger logger)
        {
            _messageBusSettings = messageBusSettings;
            _logger = logger;
        }

        private async Task SendAsyncInternal(IEnumerable<CloudEventRequest> requests)
        {
            var requestEntries =
                requests
                    .Select((x, index) =>
                        new SendMessageBatchRequestEntry(index.ToString(), JsonSerializer.Serialize(x))
                        {
                            MessageGroupId = _messageBusSettings.KeepTheOrderOfMessages.HasValue && _messageBusSettings.KeepTheOrderOfMessages.Value
                                ? _messageBusSettings.Name
                                : $"{_messageBusSettings.Name}-{Guid.NewGuid()}"
                        })
                    .ToList();

            var request = new SendMessageBatchRequest
            {
                QueueUrl = _messageBusSettings.Link,
                Entries = requestEntries
            };

            _logger.LogInformation($"SQS: sending multiple batched : {requestEntries.Count} messages");
            var awsSqsClient = new AmazonSQSClient();
            var response = await awsSqsClient.SendMessageBatchAsync(request);
            _logger.LogInformation($"SQS: {response.Successful.Count} messages sent");

            foreach (var failed in response.Failed)
            {
                _logger.LogError($"SQS: failed messageId: {failed.Id}, Code: {failed.Code}, Message: {failed.Message}, SenderFault: {failed.SenderFault}");
            }
        }

        public async Task SendAsync(CloudEventRequest[] requests)
        {
            await SendAsyncInternal(requests);
        }

        public async Task SendAsync(CloudEventRequest request)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _messageBusSettings.Link,
                MessageBody = JsonSerializer.Serialize(request), //JsonConvert.SerializeObject(request),
                MessageGroupId = _messageBusSettings.KeepTheOrderOfMessages.HasValue && _messageBusSettings.KeepTheOrderOfMessages.Value
                    ? _messageBusSettings.Name
                    : $"{_messageBusSettings.Name}-{Guid.NewGuid()}"
            };

            var awsSqsClient = new AmazonSQSClient();
            await awsSqsClient.SendMessageAsync(sendMessageRequest);

            _logger.LogInformation(
                $"SQS: Message Sent to '{_messageBusSettings.Name}' (keepOrder: {_messageBusSettings.KeepTheOrderOfMessages})");
        }
    }
}