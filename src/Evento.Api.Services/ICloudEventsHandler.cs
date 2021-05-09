using System.Threading.Tasks;
using CloudNative.CloudEvents;

namespace Evento.Api.Services
{
    public interface ICloudEventsHandler
    {
        Task<string> Create(CloudEvent request);
    }
}
