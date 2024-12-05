using System.Text.Json.Serialization;

namespace SmallApiToolkit.Core.Response
{
    public class DataResponse<T> : ProblemResponse
    {
        [JsonPropertyOrder(-3)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; init; }

        [JsonPropertyOrder(-2)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonExtensionData]
        public IDictionary<string, object?> Extensions { get; set; } = new Dictionary<string, object?>(StringComparer.Ordinal);
    }

    public class ProblemResponse
    {
        [JsonPropertyOrder(-1)]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ErrorDetails? Error { get; init; }
    }
}
