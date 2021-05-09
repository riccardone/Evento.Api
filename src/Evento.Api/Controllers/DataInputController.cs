using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using Evento.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        private readonly ICloudEventsHandler _cloudEventsHandler;

        /// <summary>
        /// Build controller
        /// </summary>
        public DataInputController(ICloudEventsHandler cloudEventsHandler)
        {
            _cloudEventsHandler = cloudEventsHandler;
        }

        /// <summary>
        /// Ingest message synchronously
        /// </summary>
        /// <response code="201">The message has been successfully ingested</response>
        /// <response code="404">Unable to ingest the message</response>
        /// <param name="request">The CloudEvent message containing the payload and other message information</param>
        [HttpPost]
        [ProducesResponseType(typeof(CloudEvent), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody][Required] CloudEvent request)
        {
            try
            {
                var result = await _cloudEventsHandler.Create(request);
                return Ok(new { CorrelationId = result });
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
