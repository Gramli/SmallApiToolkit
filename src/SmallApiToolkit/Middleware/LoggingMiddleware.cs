using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SmallApiToolkit.Middleware
{
    public class LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        protected readonly ILogger<LoggingMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            LogRequest(context.Request);
            await _next(context);
            LogResponse(context.Response);
        }

        protected virtual void LogRequest(HttpRequest request)
        {
            
        }

        protected virtual void LogResponse(HttpResponse response) 
        { 
        
        }
    }
}
