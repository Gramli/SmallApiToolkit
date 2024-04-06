namespace SmallApiToolkit.Response
{
    public class DataResponse<T>
    {
        public T? Data { get; init; }

        public IEnumerable<string> Errors { get; init; } = [];
    }
}
