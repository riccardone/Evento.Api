using Finbuckle.MultiTenant;

namespace Evento.Api.Model
{
    public class EventoTenantInfo : TenantInfo
    {
        public string MessageBusLink { get; set; }
        public string CryptoKey { get; set; }
        public string ApiKey { get; set; }
        public string AdminEmail { get; set; }
        public string Domain { get; set; }
        public bool IngestInvalidPayloads { get; set; }
        public string ValidationErrorField { get; set; }
        public bool? KeepOrderOfMessages { get; set; }
    }
}
