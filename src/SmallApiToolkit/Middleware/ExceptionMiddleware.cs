using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SmallApiToolkit.Response;
using System.Net;
using System.Text.Json;

namespace SmallApiToolkit.Middleware
{
    //TODO ALLOW TO INHERITE AND SPECIFY CUSTOM METHODS
    public sealed class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        private readonly ILogger<ExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception generalEx)
            {
                _logger.LogError(generalEx, "Unexpected Error Occurred.");
                await WriteResponseAsync(generalEx, context);
            }
        }

        private async Task WriteResponseAsync(Exception generalEx, HttpContext context)
        {
            var (responseCode, responseMessage) = ExtractFromException(generalEx);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)responseCode;
            var jsonResult = CreateResponseJson(responseMessage);
            await context.Response.WriteAsync(jsonResult);
        }

        private string CreateResponseJson(string errorMessage)
        {
            var response = new DataResponse<object>()
            {
                Errors = new List<string>() { errorMessage }
            };
            return JsonSerializer.Serialize(response);
        }

        private (HttpStatusCode responseCode, string responseMessage) ExtractFromException(Exception generalEx)
            => generalEx switch
            {
                TaskCanceledException taskCanceledException => (HttpStatusCode.NoContent, taskCanceledException.Message),
                _ => (HttpStatusCode.InternalServerError, "Generic error occurred on server. Check logs for more info.")
            };
    }
}
