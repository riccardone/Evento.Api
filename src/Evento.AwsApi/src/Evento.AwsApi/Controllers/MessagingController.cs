using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudEventData;
using Evento.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Evento.AwsApi.Controllers
{
    [Route("api/[controller]")]
    public class MessagingController : ControllerBase
    {
        private readonly ILogger<MessagingController> _logger;
        private readonly ICloudEventsHandler _cloudEventsHandler;

        public MessagingController(ICloudEventsHandler cloudEventsProcessor, ILogger<MessagingController> logger)
        {
            _cloudEventsHandler = cloudEventsProcessor;
            _logger = logger;
        }

        /// <summary>
        /// Ingest message synchronously
        /// </summary>
        /// <response code="202">The message has been accepted and will be processed later</response>
        /// <response code="404">Unable to ingest the message</response>
        /// <response code="400">Malformed or invalid message</response>
        /// <param name="request">The CloudEvent message containing the payload and other message information</param>
        [HttpPost]
        [ProducesResponseType(typeof(CloudEventRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] [Required] CloudEventRequest request)
        {
            var result = await _cloudEventsHandler.Process(request);
            return Accepted(new { id = result });
        }

        [HttpPost]
        [Route("batch")]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CloudEventRequest), StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status408RequestTimeout)]
        public async Task<IActionResult> CreateBatchAsync([FromBody][Required] CloudEventRequest[] value)
        {
            var result = await _cloudEventsHandler.Process(value);
            return Accepted(new { id = result });
        }
    }
}