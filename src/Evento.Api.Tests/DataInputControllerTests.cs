using Evento.Api.Contracts;
using Evento.Api.Controllers;
using Evento.Api.Services;
using Evento.Api.Tests.Fakes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;

namespace Evento.Api.Tests
{
    public class DataInputControllerTests
    {
        private DataInputController BuildInsuringController(IMessageSender messageSender, IPayloadValidator payloadValidator, bool ingestInvalidPayloads)
        {
            return new DataInputController(new CloudEventsHandler(new FakeMultitenantStore(ingestInvalidPayloads),
                new IdGenerator(),
                new PayloadValidator(new ResourceElements(),
                    new SchemaProvider(
                        new AppSettings() {Schema = new Schema {File = "schema.json", PathRoot = "Schemas"}},
                        new FileLocator())), new FakeMessageSenderFactory(new MessageSenderInMemory()),
                new NullLogger<CloudEventsHandler>()), new NullLogger<DataInputController>());
        }

        [Test]
        public void If_IngestInvalidPayload_setting_is_true_I_Expect_to_Ingest_Invalid_Payloads_With_No_Error()
        {
            // Setup
            var sut = BuildInsuringController(new MessageSenderInMemory(), new FakePayloadValidatorForInvalidPayloads(), true);

            // Act
            var result = sut.Create(Helpers.BuildCloudRequest("creatediary/1.0", "myselflog", "creatediary", Helpers.SampleJsonForInvalidRequest)).Result;

            // Verify
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public void If_IngestInvalidPayload_setting_is_true_And_the_payload_has_already_a_ValidationError_I_Expect_to_Ingest_Invalid_Payloads_With_No_Error()
        {
            // Setup
            var sut = BuildInsuringController(new MessageSenderInMemory(), new FakePayloadValidatorForInvalidPayloads(), true);

            // Act
            var result = sut.Create(Helpers.BuildCloudRequest("creatediary/1.0", "myselflog", "creatediary", Helpers.SampleJsonForInvalidRequestWithExistingValidationError)).Result;

            // Verify
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }

        [Test]
        public void If_IngestInvalidPayload_setting_is_false_I_Expect_to_not_Ingest_Invalid_Payloads()
        {
            // Setup
            var sut = BuildInsuringController(new MessageSenderInMemory(), new FakePayloadValidatorForInvalidPayloads(), false);

            // Act
            var result = sut.Create(Helpers.BuildCloudRequest("creatediary/1.0", "myselflog", "creatediary", Helpers.SampleJsonForInvalidRequest)).Result;

            // Verify
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }
    }
}