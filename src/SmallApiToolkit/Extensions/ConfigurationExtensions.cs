using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace SmallApiToolkit.Extensions
{
    public static class ConfigurationExtensions
    {
        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {
            builder.Logging
                .ClearProviders()
                .AddConsole();

            return builder;
        }
    }
}
