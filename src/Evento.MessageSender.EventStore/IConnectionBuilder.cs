using EventStore.ClientAPI;

namespace Evento.MessageSender.EventStore
{
    public interface IConnectionBuilder
    {
        IEventStoreConnection Build();
    }
}
