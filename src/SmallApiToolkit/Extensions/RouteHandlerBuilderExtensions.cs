using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using SmallApiToolkit.Core.Response;

namespace SmallApiToolkit.Extensions
{
    public static class RouteHandlerBuilderExtensions
    {
        public static RouteHandlerBuilder ProducesDataResponse<TResponse>(
            this RouteHandlerBuilder builder,
            int statusCode = StatusCodes.Status200OK,
            string? contentType = null,
            params string[] additionalContentTypes)
        => builder.Produces<DataResponse<TResponse>>(statusCode, contentType, additionalContentTypes);
    }
}
