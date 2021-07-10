using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CloudEventData;
using Crypto;
using Evento.Api.Contracts;
using Evento.Api.Model;
using Finbuckle.MultiTenant;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Evento.Api.Services
{
    public class CloudEventsHandler : ICloudEventsHandler
    {
        private readonly ILogger<CloudEventsHandler> _logger;
        private readonly IMessageSenderFactory _messageSenderFactory;
        private readonly IPayloadValidator _payloadValidator;
        private readonly IIdGenerator _idGenerator;
        private readonly IMultiTenantStore<EventoTenantInfo> _store;

        public CloudEventsHandler(IMultiTenantStore<EventoTenantInfo> store, IIdGenerator idGenerator,
            IPayloadValidator payloadValidator, IMessageSenderFactory messageSenderFactory,
            ILogger<CloudEventsHandler> logger)
        {
            _store = store;
            _idGenerator = idGenerator;
            _payloadValidator = payloadValidator;
            _messageSenderFactory = messageSenderFactory;
            _logger = logger;
        }

        private const string IdField = "CorrelationId";
        public async Task<string> Process(CloudEventRequest request)
        {
            if (request.DataContentType == null || !request.DataContentType.Equals("application/json"))
                throw new Exception("DataContentType must be: 'application/json'");

            if (string.IsNullOrWhiteSpace(request.Type))
                throw new Exception("Type must be set");

            var tenant = _store.TryGetByIdentifierAsync(request.Source.ToString()).Result;
            if (tenant == null)
                throw new Exception("Tenant not recognised");

            var isValid = IsValid(request, tenant, out var err);
            if (!isValid && !tenant.IngestInvalidPayloads)
                throw new Exception(err);

            var data = JsonConvert.DeserializeObject<JObject>(request.Data.ToString());

            IngestInvalidPayloadIfNecessary(request, tenant, isValid, data, err);

            var result = CheckIfMessageNeedCorrelationId(request, data, request.Id);

            EncryptMessageIfNeeded(request);

            var sender = _messageSenderFactory.Build(request.Source.ToString());
            await sender.SendAsync(request);

            return result;
        }

        private void IngestInvalidPayloadIfNecessary(CloudEventRequest request, EventoTenantInfo tenant, bool isValid, JObject data,
            string err)
        {
            if (!tenant.IngestInvalidPayloads || isValid) return;
            _logger.LogWarning($"Ingesting invalid payload for tenant: '{tenant.Name}'");
            var hasValidationErrorField = false;
            foreach (var property in data)
            {
                if (!property.Key.Equals(tenant.ValidationErrorField)) continue;
                hasValidationErrorField = true;
                _logger.LogWarning(
                    $"Invalid payload has an existing validation error. Previous error was: {property.Value}");
                break;
            }

            if (!hasValidationErrorField)
                data.Add(tenant.ValidationErrorField, err);
            else
                data[tenant.ValidationErrorField] = err;
            request.Data = data;
        }

        private bool IsValid(CloudEventRequest request, EventoTenantInfo tenant, out string error)
        {
            error = null;
            if (request.DataSchema != null && request.DataSchema.IsWellFormedOriginalString())
            {
                var validationResult = _payloadValidator.Validate(request.DataSchema.ToString(), request.Data.ToString());

                if (!validationResult.IsValid)
                {
                    string errors = string.Join(',', validationResult.ErrorMessages);
                    _logger.LogWarning(
                        $"Request received with invalid schema (id:{request.Id};source:{request.Source};type:{request.Type};dataSchema:{request.DataSchema};errors:{errors})");

                    var settings = new JsonSerializerSettings
                    {
                        StringEscapeHandling = StringEscapeHandling.EscapeHtml
                    };

                    error = JsonConvert.SerializeObject(new { Error = errors.Replace("'", "") }, settings);
                    return false;
                }

                _logger.LogInformation(
                    $"Request received with valid schema (id:{request.Id};source:{request.Source};type:{request.Type};dataSchema:{request.DataSchema})");
            }
            else
                _logger.LogInformation(
                    $"Request received without schema (id:{request.Id};source:{request.Source};type:{request.Type})");

            return true;
        }

        private string CheckIfMessageNeedCorrelationId(CloudEventRequest request, JObject data, string result)
        {
            var hasIdentifier = false;
            foreach (KeyValuePair<string, JToken> property in data)
            {
                if (!property.Key.ToLower().Equals(IdField.ToLower())) continue;
                hasIdentifier = true;
                result = property.Value.ToString();
                break;
            }

            if (!hasIdentifier)
            {
                var id = new JValue(_idGenerator.GenerateId(string.Empty));
                data.Add(IdField, id);
                result = id.Value.ToString();
                request.Data = data;
            }

            return result;
        }

        private void EncryptMessageIfNeeded(CloudEventRequest request)
        {
            var tenant = _store.TryGetByIdentifierAsync(request.Source.ToString()).Result;
            if (tenant == null || string.IsNullOrWhiteSpace(tenant.CryptoKey))
                return;
            var cryptoService = new AesCryptoService(Convert.FromBase64String(tenant.CryptoKey));
            request.Data = Convert.ToBase64String(cryptoService.Encrypt(JsonConvert.SerializeObject(request.Data)));
            // Example to decrypt
            //var test = cryptoService.Decrypt( Convert.FromBase64String(request.Data));
        }
    }
}
