using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace SmallApiToolkit.Extensions
{
    public static class HttpResponseCachingExtensions
    {
        public static RouteHandlerBuilder AddResponseCacheHeader(this RouteHandlerBuilder routeHandlerBuilder, int maxAgeInSeconds)
          => routeHandlerBuilder.AddEndpointFilter(async (context, next) =>
          {
              var isGetOrHeadRequest = HttpMethods.IsGet(context.HttpContext.Request.Method) || 
                                       HttpMethods.IsHead(context.HttpContext.Request.Method);
              if (!isGetOrHeadRequest)
              {
                  return await next(context);
              }

              var result = await next(context);
              
              if (context.HttpContext.Response.StatusCode == StatusCodes.Status200OK)
              {
                  context.HttpContext.Response.Headers.CacheControl = $"public,max-age={maxAgeInSeconds}";
              }
              
              return result;
          });
    }
}
