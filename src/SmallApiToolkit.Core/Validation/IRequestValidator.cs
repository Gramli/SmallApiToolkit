namespace SmallApiToolkit.Core.Validation
{
    public interface IRequestValidator<TRequest>
    {
        RequestValidationResult Validate(TRequest request);
    }
}
