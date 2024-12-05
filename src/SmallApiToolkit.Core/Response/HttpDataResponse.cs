using System.Net;
using System.Text.Json.Serialization;

namespace SmallApiToolkit.Core.Response
{
    public class HttpDataResponse<T> : DataResponse<T>
    {
        [JsonIgnore]
        public HttpStatusCode StatusCode { get; init; }
    }
}
