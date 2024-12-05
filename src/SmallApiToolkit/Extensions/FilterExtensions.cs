using Microsoft.Extensions.DependencyInjection;
using SmallApiToolkit.Filters;

namespace SmallApiToolkit.Extensions
{
    public static class FilterExtensions
    {
        public static IServiceCollection AddProblemDetails(this IServiceCollection services, params int[] statusCodes)
            => 
            services.Configure((ProblemDetailsFilterOptions options) =>
            {
                options.ErrorStatusCode = statusCodes;
            })
            .AddProblemDetails();

        public static IServiceCollection AddProblemDetails(this IServiceCollection services, Action<ProblemDetailsFilterOptions> configure)
            => 
            services.Configure(configure)
            .AddProblemDetails();

    }
}
