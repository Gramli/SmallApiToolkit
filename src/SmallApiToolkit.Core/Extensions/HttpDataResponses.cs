using SmallApiToolkit.Core.Response;
using System.Net;

namespace SmallApiToolkit.Core.Extensions
{
    public static class HttpDataResponses
    {
        public static HttpDataResponse<T> AsBadRequest<T>(IEnumerable<string> errorMessages)
            => AsResponse<T>(HttpStatusCode.BadRequest, errorMessages);

        public static HttpDataResponse<T> AsBadRequest<T>(string errorMessages)
            => AsResponse<T>(HttpStatusCode.BadRequest, errorMessages);

        public static HttpDataResponse<T> AsForbidden<T>(IEnumerable<string> errorMessages)
            => AsResponse<T>(HttpStatusCode.Forbidden, errorMessages);

        public static HttpDataResponse<T> AsForbidden<T>(string errorMessages)
            => AsResponse<T>(HttpStatusCode.Forbidden, errorMessages);

        public static HttpDataResponse<T> AsUnauthorized<T>(IEnumerable<string> errorMessages)
            => AsResponse<T>(HttpStatusCode.Unauthorized, errorMessages);

        public static HttpDataResponse<T> AsUnauthorized<T>(string errorMessages)
            => AsResponse<T>(HttpStatusCode.Unauthorized, errorMessages);

        public static HttpDataResponse<T> AsInternalServerError<T>(IEnumerable<string> errorMessages)
            => AsResponse<T>(HttpStatusCode.InternalServerError, errorMessages);

        public static HttpDataResponse<T> AsInternalServerError<T>(string errorMessage)
            => AsResponse<T>(HttpStatusCode.InternalServerError, errorMessage);

        public static HttpDataResponse<T> AsOK<T>(T data)
            => new()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
            };

        public static HttpDataResponse<T> AsOK<T>(T data, IEnumerable<string> errorMessages)
            => new()
            {
                Data = data,
                StatusCode = HttpStatusCode.OK,
                Errors = errorMessages
            };

        public static HttpDataResponse<T> AsNoContent<T>()
            => AsResponse<T>(HttpStatusCode.NoContent);

        public static HttpDataResponse<T> AsNotFound<T>()
            => AsResponse<T>(HttpStatusCode.NotFound);

        public static HttpDataResponse<T> AsNotFound<T>(IEnumerable<string> errorMessages)
            => AsResponse<T>(HttpStatusCode.NotFound, errorMessages);

        public static HttpDataResponse<T> AsNotFound<T>(string errorMessages)
            => AsResponse<T>(HttpStatusCode.NotFound, errorMessages);

        public static HttpDataResponse<T> AsAccepted<T>()
            => AsResponse<T>(HttpStatusCode.Accepted);

        public static HttpDataResponse<T> AsAccepted<T>(IEnumerable<string> errorMessages)
            => AsResponse<T>(HttpStatusCode.Accepted, errorMessages);

        public static HttpDataResponse<T> AsAccepted<T>(string errorMessages)
            => AsResponse<T>(HttpStatusCode.Accepted, errorMessages);

        private static HttpDataResponse<T> AsResponse<T>(HttpStatusCode httpStatusCode)
            => new()
            {
                StatusCode = httpStatusCode,
            };

        private static HttpDataResponse<T> AsResponse<T>(HttpStatusCode httpStatusCode, IEnumerable<string> errorMessages)
            => new()
            {
                StatusCode = httpStatusCode,
                Errors = errorMessages
            };

        private static HttpDataResponse<T> AsResponse<T>(HttpStatusCode httpStatusCode, string errorMessage)
            => new()
            {
                StatusCode = httpStatusCode,
                Errors = [errorMessage]
            };

    }
}
