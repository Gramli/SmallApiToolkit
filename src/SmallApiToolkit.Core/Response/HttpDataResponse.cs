using System.Net;

namespace SmallApiToolkit.Core.Response
{
    public class HttpDataResponse<T> : DataResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }
    }
}
