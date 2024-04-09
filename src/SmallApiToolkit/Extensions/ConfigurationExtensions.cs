using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace SmallApiToolkit.Extensions
{
    public static class ConfigurationExtensions
    {
        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder, LogLevel minimumLevel = LogLevel.Information)
        {
            builder.Logging
                .ClearProviders()
                .AddConsole()
                .SetMinimumLevel(minimumLevel);

            return builder;
        }
    }
}
