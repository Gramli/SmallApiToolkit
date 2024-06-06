namespace SmallApiToolkit.Core.Validation
{
    public interface IRequestValidator<TRequest>
    {
        bool IsValid(TRequest request);
    }
}
