using System.Threading.Tasks;
using CloudEventData;

namespace Evento.Api.Services
{
    public interface ICloudEventsHandler
    {
        Task<string> Create(CloudEventRequest request);
    }
}
