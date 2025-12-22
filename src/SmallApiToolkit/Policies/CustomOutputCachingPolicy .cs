using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;

namespace SmallApiToolkit.Policies
{
    internal static class CustomOutputCachingPolicyFactory
    {
        internal static CustomOutputCachingPolicy CreateOutputCachingPolicy(TimeSpan timeSpan)
            => new(timeSpan);
    }

    internal sealed class CustomOutputCachingPolicy : IOutputCachePolicy
    {
        private readonly TimeSpan _responseExpirationTime;
        internal CustomOutputCachingPolicy(TimeSpan responseExpirationTime)
        {
            _responseExpirationTime = responseExpirationTime;
        }

        ValueTask IOutputCachePolicy.CacheRequestAsync(
            OutputCacheContext context,
            CancellationToken cancellation)
        {
            var attemptOutputCaching = AttemptOutputCaching(context);
            context.EnableOutputCaching = attemptOutputCaching;
            context.AllowCacheLookup = attemptOutputCaching;
            context.AllowCacheStorage = attemptOutputCaching;
            context.AllowLocking = true;
            context.ResponseExpirationTimeSpan = _responseExpirationTime;
            context.CacheVaryByRules.QueryKeys = "*";

            return ValueTask.CompletedTask;
        }

        ValueTask IOutputCachePolicy.ServeFromCacheAsync
            (OutputCacheContext context, CancellationToken cancellation)
            => ValueTask.CompletedTask;

        ValueTask IOutputCachePolicy.ServeResponseAsync
            (OutputCacheContext context, CancellationToken cancellation)
        {
            var response = context.HttpContext.Response;

            if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            if (response.StatusCode is not (StatusCodes.Status200OK or StatusCodes.Status301MovedPermanently))
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            return ValueTask.CompletedTask;
        }

        private static bool AttemptOutputCaching(OutputCacheContext context)
        {
            var request = context.HttpContext.Request;

            return HttpMethods.IsGet(request.Method) ||
                HttpMethods.IsHead(request.Method);

        }
    }
}
