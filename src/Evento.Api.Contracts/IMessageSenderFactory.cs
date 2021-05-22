namespace Evento.Api.Contracts
{
    public interface IMessageSenderFactory
    {
        IMessageSender Build(string source);
    }
}
