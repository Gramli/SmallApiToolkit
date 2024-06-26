﻿using SmallApiToolkit.Core.Response;

namespace SmallApiToolkit.Core.RequestHandlers
{
    public interface IHttpRequestHandler<TResponse, in TRequest> : IRequestHandler<HttpDataResponse<TResponse>, TRequest>
    {
    }
}
