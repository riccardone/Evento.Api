using System;
using System.IO;
using Evento.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Evento.AwsApi.Controllers
{
    [Route("api/[controller]")]
    public class SchemaController : ControllerBase
    {
        private readonly ISchemaProvider _schemaProvider;

        public SchemaController(ISchemaProvider schemaProvider)
        {
            _schemaProvider = schemaProvider;
        }

        [HttpGet("{schema}/{version}")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAsync(string schema, string version)
        {
            try
            {
                var schemaPayload = _schemaProvider.GetSchema(schema, version);
                return Ok(schemaPayload);
            }
            catch (FileNotFoundException)
            {
                return NotFound("schema not found");
            }
        }
    }
    
    public class SchemaProvider : ISchemaProvider
    {
        private readonly IConfiguration _configuration;
        private readonly IResourceLocator _fileLocator;

        public SchemaProvider(IConfiguration configuration, IResourceLocator fileLocator)
        {
            _configuration = configuration;
            _fileLocator = fileLocator;
        }

        /// <summary>
        /// Returns the schema for the given client and version
        /// throws an exception if the file is not found
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public string GetSchema(string schema, string version)
        {
            var schemaPathRoot = _configuration.GetSection("Schema")["PathRoot"];
            var schemaFileName = _configuration.GetSection("Schema")["File"];
            var filePath = $"{schemaPathRoot}/{schema}/{version}/{schemaFileName}";

            if (_fileLocator.Exists(filePath))
            {
                return _fileLocator.ReadAllText(filePath);
            }

            if (string.IsNullOrWhiteSpace(schemaPathRoot) && string.IsNullOrWhiteSpace(schemaFileName))
            {
                throw new Exception("invalid config settings for Schema:PathRoot and Schema:File");
            }
            
            throw new FileNotFoundException("file not found");
        }
    }
}