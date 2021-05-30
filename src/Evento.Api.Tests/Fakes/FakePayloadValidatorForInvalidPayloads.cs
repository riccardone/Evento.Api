using Evento.Api.Services;

namespace Evento.Api.Tests.Fakes
{
    public class FakePayloadValidatorForInvalidPayloads : IPayloadValidator
    {
        public PayloadValidationResult Validate(string schema, object value)
        {
            return new PayloadValidationResult(false, "error for test");
        }
    }
}
