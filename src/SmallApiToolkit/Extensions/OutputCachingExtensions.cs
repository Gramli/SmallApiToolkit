using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.DependencyInjection;
using SmallApiToolkit.Policies;

namespace SmallApiToolkit.Extensions
{
    public static class OutputCachingExtensions
    {
        public static void AddCustomOutputCachingPolicy(this OutputCacheOptions options, params (string name, TimeSpan timeSpan)[] policies)
        {
            foreach (var (name, timeSpan) in policies)
            {
                options.AddPolicy(name, CustomOutputCachingPolicyFactory.CreateOutputCachingPolicy(timeSpan));
            }
        }

        public static IServiceCollection AddOutputCacheWithCustomPolicy(
            this IServiceCollection serviceCollection,
            params (string name, TimeSpan timeSpan)[] policies)
                => serviceCollection.AddOutputCache(options =>
                {
                    options.AddCustomOutputCachingPolicy(policies);
                });

        public static IServiceCollection AddOutputCacheWithCustomPolicy(
            this IServiceCollection serviceCollection,
            Action<OutputCacheOptions> configureOptions,
            params (string name, TimeSpan timeSpan)[] policies)
                => serviceCollection.AddOutputCache(options =>
                {
                    options.AddCustomOutputCachingPolicy(policies);
                    configureOptions.Invoke(options);
                });

        public static RouteHandlerBuilder CustomCacheOutput(this RouteHandlerBuilder routeHandlerBuilder, string name)
            => routeHandlerBuilder.CacheOutput(name);
    }
}
