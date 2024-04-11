using SmallApiToolkit.Core.Response;
using System.Net;

namespace SmallApiToolkit.Core.Extensions
{
    public static class HttpDataResponses
    {
        public static HttpDataResponse<T> AsBadRequest<T>(IEnumerable<string> errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsBadRequest<T>(string errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Errors = [errorMessages]
            };
        }

        public static HttpDataResponse<T> AsInternalServerError<T>(IEnumerable<string> errorMessages)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsInternalServerError<T>(string errorMessage)
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Errors = [errorMessage]
            };
        }

        public static HttpDataResponse<T> AsOK<T>(T data)
        {
            return new HttpDataResponse<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };
        }

        public static HttpDataResponse<T> AsOK<T>(T data, IEnumerable<string> errorMessages)
        {
            return new HttpDataResponse<T>
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Errors = errorMessages
            };
        }

        public static HttpDataResponse<T> AsNoContent<T>()
        {
            return new HttpDataResponse<T>
            {
                StatusCode = HttpStatusCode.NoContent,
            };
        }
    }
}
