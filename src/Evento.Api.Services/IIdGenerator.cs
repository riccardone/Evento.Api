namespace Evento.Api.Services
{
    public interface IIdGenerator
    {
        string GenerateId(string prefix);
    }

    public interface IIdWriter
    {
        void Set(string id);
    }
}