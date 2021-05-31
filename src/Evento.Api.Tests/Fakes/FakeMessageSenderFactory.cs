using Evento.Api.Contracts;

namespace Evento.Api.Tests.Fakes
{
    public class FakeMessageSenderFactory : IMessageSenderFactory
    {
        private readonly IMessageSender _messageSender;

        public FakeMessageSenderFactory(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }
        public IMessageSender Build(string source)
        {
            return _messageSender;
        }
    }
}