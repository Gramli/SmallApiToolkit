using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Core.Extensions;
using SmallApiToolkit.Core.RequestHandlers;
using SmallApiToolkit.Core.Response;
using SmallApiToolkit.Core.Validation;
using SmallApiToolkit.Extensions;

namespace TestWebApplication.WeatherForecast
{

    public static class WeatherForecastEndpointBuilder
    {
        public static IEndpointRouteBuilder AddForecastEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/weatherforecastException",
                async (IHttpRequestHandler<bool, EmptyRequest> httpRequestHandler, CancellationToken cancellationToken) =>
                    await httpRequestHandler.SendAsync(EmptyRequest.Instance, cancellationToken))
                    .ProducesDataResponse<WeatherForecastDto[]>();

            endpointRouteBuilder.MapGet("/weatherforecast",
                async (IHttpRequestHandler<WeatherForecastDto[], EmptyRequest> httpRequestHandler, CancellationToken cancellationToken) =>
                    await httpRequestHandler.SendAsync(EmptyRequest.Instance, cancellationToken))
                    .ProducesDataResponse<WeatherForecastDto[]>();

            endpointRouteBuilder.MapGet("/weatherforecastValidate",
                async ([FromQuery(Name = "temp")]int temperatureC, [FromQuery(Name = "sum")]string summary, IHttpRequestHandler<WeatherForecastDto[], WeatherForecastRequestDto> httpRequestHandler, CancellationToken cancellationToken) =>
                    await httpRequestHandler.SendAsync(new WeatherForecastRequestDto(temperatureC, summary), cancellationToken))
                    .ProducesDataResponse<WeatherForecastDto[]>();

            return endpointRouteBuilder;
        }
    }

    public static class WeatherForecastConfiguration
    {
        public static IServiceCollection ConfigureWeatherForecast(this IServiceCollection serviceDescriptors)
            => serviceDescriptors
                .AddScoped<IHttpRequestHandler<WeatherForecastDto[], EmptyRequest>, WeatherForecastRequestHandler>()
                .AddScoped<IHttpRequestHandler<WeatherForecastDto[], WeatherForecastRequestDto>, WeatherForecastValidationRequestHandler>()
                .AddScoped<IRequestValidator<WeatherForecastRequestDto>, WeatherForecastValidationRequestValidator>()
                .AddScoped<IHttpRequestHandler<bool, EmptyRequest>, WeatherForecastExceptionRequestHandler>();
    }

    internal class WeatherForecastRequestHandler : IHttpRequestHandler<WeatherForecastDto[], EmptyRequest>
    {
        public Task<HttpDataResponse<WeatherForecastDto[]>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecastDto
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
            return Task.FromResult(HttpDataResponses.AsOK(forecast));
        }
    }

    internal class WeatherForecastExceptionRequestHandler : IHttpRequestHandler<bool, EmptyRequest>
    {
        public Task<HttpDataResponse<bool>> HandleAsync(EmptyRequest request, CancellationToken cancellationToken)
        {
            throw new Exception("Test exception");
        }
    }

    internal class WeatherForecastValidationRequestHandler : ValidationHttpRequestHandler<WeatherForecastDto[], WeatherForecastRequestDto>
    {
        public WeatherForecastValidationRequestHandler(IRequestValidator<WeatherForecastRequestDto> validator) 
            : base(validator)
        {
        }

        protected override Task<HttpDataResponse<WeatherForecastDto[]>> HandleValidRequestAsync(WeatherForecastRequestDto request, CancellationToken cancellationToken)
        {
            var summaries = new[]
{
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };

            var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecastDto
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();
            return Task.FromResult(HttpDataResponses.AsOK(forecast));
        }
    }

    public class WeatherForecastValidationRequestValidator : IRequestValidator<WeatherForecastRequestDto>
    {
        public RequestValidationResult Validate(WeatherForecastRequestDto request)
        {
            if (request.TemperatureC > 999)
            {
                return new RequestValidationResult { IsValid = false };
            }
            return new RequestValidationResult { IsValid = true };
        }
    }

    public record WeatherForecastDto(DateOnly Date, int TemperatureC, string? Summary)
    {
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    }

    public record WeatherForecastRequestDto(int TemperatureC, string Summary) { }
}
