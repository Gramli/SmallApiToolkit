using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace SmallApiToolkit.Extensions
{
    public static class ApiVersionExtensions
    {
        public static IEndpointRouteBuilder MapVersionGroup(this IEndpointRouteBuilder builder, int version)
        => builder
            .MapGroup($"v{version}");
    }
}
