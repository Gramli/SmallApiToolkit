using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmallApiToolkit.Extensions
{
    public static class CorsExtensions
    {
        public static string AddCorsByConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var name = configuration["CORS:name"];
            var urls = configuration.GetSection("CORS:urls").Get<string[]>();

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (urls is null || urls.Length == 0)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: name, policy =>
                    {
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });
            }
            else
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(name: name, policy =>
                    {
                        policy.WithOrigins(urls)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                });
            }

            return name;
        }
    }
}
