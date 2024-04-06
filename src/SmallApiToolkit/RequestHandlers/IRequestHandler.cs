using SmallApiToolkit.Response;

namespace SmallApiToolkit.RequestHandlers
{
    public interface IRequestHandler<TResponse, in TRequest>
    {
        Task<HttpDataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
