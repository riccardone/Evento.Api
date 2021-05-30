namespace Evento.Api.Services
{
    public class AppSettings
    {
        public Schema Schema { get; set; }
    }

    public class Schema
    {
        public string PathRoot { get; set; }
        public string File { get; set; }
    }
}
