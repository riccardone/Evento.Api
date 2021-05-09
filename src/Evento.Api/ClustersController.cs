using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Evento.Api
{
    [Route("api/[controller]")]
    public class ClustersController 
    {
        // POST
        [HttpPost]
        public IActionResult CreateCluster([FromBody]JObject action)
        {
            return new JsonResult("{'greeting': 'ciao ciao lulu'}");
        }
    }
}