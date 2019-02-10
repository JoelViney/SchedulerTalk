using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SchedulerTalk.Models
{
    /// <summary>
    /// Interjects itself when a controller throws an exception and converts the exception into a json error.
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate next;

        /// <summary>Used to process http requests.</summary>
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            this._logger = logger;
            this.next = next;
        }

        /// <summary>
        /// This is called when processing errors within the API.
        /// </summary>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            HttpStatusCode code;

            if (ex is NotFoundException)
                code = HttpStatusCode.NotFound;
            else if (ex is UnauthorizedException)
                code = HttpStatusCode.Unauthorized;
            else if (ex is ValidationException)
                code = HttpStatusCode.BadRequest;
            else if (ex is NotImplementedException)
                code = HttpStatusCode.NotImplemented;
            else
                code = HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            string message = ex.Message;

            if (message.Contains(" See the inner exception for details.") && ex.InnerException != null)
            {
                message = message.Replace(" See the inner exception for details.", "");
                message = $"{message} {ex.InnerException.Message}";
            }
            var result = JsonConvert.SerializeObject(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}
