namespace SmallApiToolkit.Core.RequestHandlers
{
    public interface IRequestHandler<TResponse, in TRequest>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }

    public interface IRequestHandler<TResponse>
    {
        Task<TResponse> HandleAsync(CancellationToken cancellationToken);
    }
}
