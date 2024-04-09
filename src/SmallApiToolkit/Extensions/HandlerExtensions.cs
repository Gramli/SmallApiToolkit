using Microsoft.AspNetCore.Http;
using SmallApiToolkit.File;
using SmallApiToolkit.RequestHandlers;
using SmallApiToolkit.Response;

namespace SmallApiToolkit.Extensions
{
    public static class HandlerExtensions
    {
        public static async Task<IResult> SendAsync<TResponse, TRequest>(this IRequestHandler<TResponse, TRequest> requestHandler, TRequest request, CancellationToken cancellationToken)
        {
            var response = await requestHandler.HandleAsync(request, cancellationToken);
            return Results.Json((DataResponse<TResponse>)response, statusCode: (int)response.StatusCode);
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
