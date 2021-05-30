using System;
using CloudEventData;
using Newtonsoft.Json.Linq;

namespace Evento.Api.Tests
{
    public class Helpers
    {
        public static readonly string SampleJsonForInvalidRequest = @"{ 'AnId':'1234' }";
        public static readonly string SampleJsonForInvalidRequestWithExistingValidationError = @"{ 'ValidationError': '{\'Error\':\'String 2020-5-8 does not validate against format date. Path TransactionDate, line 4, position 31.,Required properties are missing from object: Whatever, ProductLevel. Path , line 1, position 1.\'}' }";

        public static CloudEventRequest BuildCloudRequest(string dataSchema, string source, string type, dynamic data, string id = "123", string dataContentType = "application/json")
        {
            var result = new CloudEventRequest
            {
                DataSchema = new Uri(dataSchema, UriKind.RelativeOrAbsolute),
                Source = new Uri(source, UriKind.RelativeOrAbsolute),
                Type = type,
                Time = DateTime.Now,
                DataContentType = dataContentType,
                Id = id,
                Data = JObject.Parse(data)
            };
            return result;
        }
    }
}
