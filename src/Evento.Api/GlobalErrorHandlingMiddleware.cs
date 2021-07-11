using System;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Evento.Api.Services;
using Microsoft.AspNetCore.Http;
using NLog;

namespace Evento.Api
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public GlobalErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (ex)
                {
                    case BusinessException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var errorResponse = new
                {
                    message = ex.Message,
                    statusCode = response.StatusCode
                };

                var errorJson = JsonSerializer.Serialize(errorResponse);

                _logger.Error(ex);

                await response.WriteAsync(errorJson);
            }
        }
    }
}
