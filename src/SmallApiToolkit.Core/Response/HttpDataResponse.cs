using System.Net;

namespace SmallApiToolkit.Core.Response
{
    public class HttpDataResponse<T> : DataResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }

        public static HttpDataResponse<T> AsBadRequest(IEnumerable<string> errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsBadRequest(string errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [errorMessages]
            };
        }

        public static HttpDataResponse<T> AsInternalServerError(IEnumerable<string> errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsInternalServerError(string errorMessage)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [errorMessage]
            };
        }

        public static HttpDataResponse<T> AsOK(T data)
        {
            return new HttpDataResponse<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public static HttpDataResponse<T> AsOK(T data, IEnumerable<string> errorMessages)
        {
            return new HttpDataResponse<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsNoContent()
        {
            return new HttpDataResponse<T>
            {
                StatusCode = System.Net.HttpStatusCode.NoContent,
            };
        }

    }
}
