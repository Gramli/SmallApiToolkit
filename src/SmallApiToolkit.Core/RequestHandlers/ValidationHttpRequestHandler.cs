using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Core.Validation;

namespace SmallApiToolkit.Core.RequestHandlers
{
    public abstract class ValidationHttpRequestHandler<TResponse, TRequest> : IHttpRequestHandler<TResponse, TRequest>
    {
        protected virtual string BadRequestMessage { get { return "Invalid request"; } }

        protected readonly IRequestValidator<TRequest> _validator;

        protected ValidationHttpRequestHandler(IRequestValidator<TRequest> validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }
        public async Task<HttpDataResponse<TResponse>> HandleAsync(TRequest request, CancellationToken cancellationToken)
        {
            if(!_validator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<TResponse>(BadRequestMessage);
            }

            return await HandleValidRequestAsync(request, cancellationToken);
        }

        protected abstract Task<HttpDataResponse<TResponse>> HandleValidRequestAsync(TRequest request, CancellationToken cancellationToken);
    }
}
