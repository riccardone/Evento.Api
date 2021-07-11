using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudEventData;
using Evento.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Evento.Api.Controllers
{
    /// <summary>
    /// DataInput controller
    /// </summary>
    [Route("/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class DataInputController : ControllerBase
    {
        private readonly ILogger<DataInputController> _logger;
        private readonly ICloudEventsHandler _cloudEventsHandler;

        /// <summary>
        /// Build controller
        /// </summary>
        public DataInputController(ICloudEventsHandler cloudEventsHandler, ILogger<DataInputController> logger)
        {
            _cloudEventsHandler = cloudEventsHandler;
            _logger = logger;
        }

        [Route("{id}")]
        [HttpGet]
        [ProducesResponseType(typeof(JObject), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ActionName(nameof(Get))]
        public async Task<ActionResult<JObject>> Get(string id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Ingest message synchronously
        /// </summary>
        /// <response code="201">The message has been ingested</response>
        /// <response code="404">Unable to ingest the message</response>
        /// <param name="request">The CloudEvent message containing the payload and other message information</param>
        [HttpPost]
        [ProducesResponseType(typeof(CloudEventRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody][Required] CloudEventRequest request)
        {
            var result = await _cloudEventsHandler.Process(request);
            return Ok(new {id = result}); //CreatedAtAction(nameof(Get), new { id = result });
        }
    }
}
