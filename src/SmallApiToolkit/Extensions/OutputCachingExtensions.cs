using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SmallApiToolkit.Policies;

namespace SmallApiToolkit.Extensions
{
    public static class OutputCachingExtensions
    {
        public static IServiceCollection AddOutputCacheWithCustomPolicy(this IServiceCollection serviceCollection, params (string name, TimeSpan timeSpan)[] policies)
            => serviceCollection.AddOutputCache(options =>
            {
                foreach (var (name, timeSpan) in policies)
                {
                    options.AddPolicy(name, CustomOutputCachingPolicyFactory.CreateOutputCachingPolicy(timeSpan));

                }
            });
        public static RouteHandlerBuilder CustomCacheOutput(this RouteHandlerBuilder routeHandlerBuilder, string name)
            => routeHandlerBuilder.CacheOutput(name);
    }
}
