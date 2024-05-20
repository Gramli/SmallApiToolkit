using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text;

namespace SmallApiToolkit.Middleware
{
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        protected readonly ILogger<LoggingMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context.Request);
            await _next(context);
            await LogResponse(context.Response);
        }

        protected virtual async Task LogRequest(HttpRequest request)
        {
            request.EnableBuffering();

            var requestLog = new StringBuilder();
            requestLog.AppendLine("REQUEST:");
            requestLog.AppendLine($"Method: {request.Method} {request.Path}");
            requestLog.AppendLine($"Host: {request.Host}");
            requestLog.AppendLine($"Content-Type: {request.ContentType}");
            requestLog.AppendLine($"Content-Length: {request.ContentLength}");

            TryAppendHeaders(requestLog, request.Headers);

            if (request.ContentLength.HasValue && request.ContentLength > 0)
            {
                await AppendBody(requestLog, request.Body);
            }

            _logger.LogInformation(requestLog.ToString());
            request.Body.Position = 0;
        }

        protected virtual async Task LogResponse(HttpResponse response) 
        {
            var responseLog = new StringBuilder();
            responseLog.AppendLine("RESPONSE:");
            responseLog.AppendLine($"Status Code {response.StatusCode}");
            responseLog.AppendLine($"Content-Type: {response.ContentType}");
            responseLog.AppendLine($"Content-Length: {response.ContentLength}");

            TryAppendHeaders(responseLog, response.Headers);

            if (response.ContentLength.HasValue && response.ContentLength > 0)
            {
                await AppendBody(responseLog, response.Body);
            }

            _logger.LogInformation(responseLog.ToString());
        }

        private static void TryAppendHeaders(StringBuilder builder, IHeaderDictionary headers)
        {
            if (headers is not null && headers.Count > 0)
            {
                builder.AppendLine($"Headers: {string.Join(';', headers)}");
            }
        }

        public static async Task AppendBody(StringBuilder builder, Stream body)
            => builder.AppendLine($"Body: {await ReadBodyAsync(body)}");

        private async static Task<string> ReadBodyAsync(Stream stream)
        {
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            using var reader = new StreamReader(memory);
            return await reader.ReadToEndAsync();
        }
    }
}
