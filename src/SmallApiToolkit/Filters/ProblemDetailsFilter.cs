using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace SmallApiToolkit.Filters
{
    public class ProblemDetailsFilter : IEndpointFilter
    {
        public ProblemDetailsFilter(IOptions<ProblemDetailsFilterOptions> options, IProblemDetailsService problemDetailsService)
        {

        }
        public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            throw new NotImplementedException();
        }
    }
}
