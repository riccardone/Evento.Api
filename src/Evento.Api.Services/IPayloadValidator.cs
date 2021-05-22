namespace Evento.Api.Services
{
    public interface IPayloadValidator
    {
        PayloadValidationResult Validate(string schema, object value);
    }
}
