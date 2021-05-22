using Evento.Api.Contracts;

namespace Evento.Api.Services
{
    public class BusSettings : IBusSettings
    {
        public BusSettings(string link, string name)
        {
            Link = link;
            Name = name;
        }

        public string Link { get; }
        public string Name { get; }
    }
}
