using Microsoft.AspNetCore.Http;
using SmallApiToolkit.File;
using SmallApiToolkit.RequestHandlers;

namespace SmallApiToolkit.Extensions
{
    public static class HandlerExtension
    {
        public static async Task<IResult> SendAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken)
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            return Results.Json(response, statusCode: (int)response.StatusCode);
        }
        public static async Task<IResult> GetFileAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken) where TResponse : IFile
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            if (response.Data is not null)
            {
                return Results.File(response.Data.Data, response.Data.ContentType, response.Data.FileName);
            }
            return Results.NotFound();
        }
    }
}
