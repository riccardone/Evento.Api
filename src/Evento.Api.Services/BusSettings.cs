using Evento.Api.Contracts;

namespace Evento.Api.Services
{
    public class BusSettings : IBusSettings
    {
        public BusSettings(string link, string name, bool? keepTheOrderOfMessages)
        {
            Link = link;
            Name = name;
            KeepTheOrderOfMessages = keepTheOrderOfMessages;
        }

        public BusSettings(string link, string name) : this(link, name, null) { }

        public string Link { get; }
        public string Name { get; }
        public bool? KeepTheOrderOfMessages { get; }
    }
}
