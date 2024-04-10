namespace SmallApiToolkit.Core.RequestHandlers
{
    public interface IRequestHandler<TResponse, in TRequest>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
    }
}
