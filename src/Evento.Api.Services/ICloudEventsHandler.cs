using System.Threading.Tasks;
using CloudEventData;

namespace Evento.Api.Services
{
    public interface ICloudEventsHandler
    {
        Task<string> Process(CloudEventRequest request, bool validate = true);
        Task<string> Process(CloudEventRequest[] requests);
    }
}
