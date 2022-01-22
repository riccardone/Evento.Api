namespace Evento.Api.Contracts
{
    public interface IBusSettings
    {
        string Link { get; }
        string Name { get; }
        bool? KeepTheOrderOfMessages { get; }
    }
}
