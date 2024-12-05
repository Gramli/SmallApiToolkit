namespace SmallApiToolkit.Filters
{
    public class ProblemDetailsFilterOptions
    {
        public int DefaultStartErrorCode { get; } = 400;
        public int[] ErrorStatusCode { get; set; } = [];
    }
}
