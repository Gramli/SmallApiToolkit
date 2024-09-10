# SmallApiToolkit
**SmallApiToolkit** is a powerful and efficient tool designed to help developers build small APIs more quickly and with greater ease. It provides a range of features that streamline the creation of clean, maintainable, and well-structured API endpoints.

## RequestHandler
To ensure Minimal API endpoints are clear and readable, **SmallApiToolkit** offers the **IHttpRequestHandler** interface. By implementing this interface in your handler, you can utilize the **SendAsync** extension method directly within the Minimal API endpoint. This method simplifies the process by automatically generating a JSON response based on the **IHttpRequestHandler** implementation. Additionally, every response is wrapped in a **DataResponse**, ensuring a consistent structure containing **Data** and **Errors**.

```
public class DataResponse<T>
{
    public T? Data { get; init; }

    public IEnumerable<string> Errors { get; init; } = Array.Empty<string>();

}
```

Example **IHttpRequestHandler** usage:
1. Implement the IHttpRequestHandler interface

```csharp
    internal sealed class AddFavoriteHandler : IHttpRequestHandler<int, AddFavoriteCommand>
    {
        private readonly IValidator<AddFavoriteCommand> _addFavoriteCommandValidator;
        public AddFavoriteHandler()
        {
            _addFavoriteCommandValidator = Guard.Against.Null(addFavoriteCommandValidator);
        }

        public async Task<HttpDataResponse<int>> HandleAsync(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_addFavoriteCommandValidator.IsValid(request))
            {
                return HttpDataResponses.AsBadRequest<int>(string.Format(ErrorMessages.RequestValidationError, request));
            }


            return HttpDataResponses.AsOK(addResult.Value);
        }
```

2. Register Handler in DI container:

```csharp
serviceCollection.AddScoped<IHttpRequestHandler<int, AddFavoriteCommand>, AddFavoriteHandler>()
```
3. Register Minimal Api endpoint:

```csharp
MapPost("favorite", async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IHttpRequestHandler<int, AddFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .ProducesDataResponse<int>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");
```

### ValidationHttpRequestHandler

To ensure that every client-side request is validated, the **ValidationHttpRequestHandler<TResponse, TRequest>** class is provided. This class eliminates the need to duplicate validation logic across handlers by leveraging the **IRequestValidator<TRequest>** interface.

## Midlewares
Middlewares are essential for handling cross-cutting concerns such as authentication, logging, and error handling. **SmallApiToolkit** provides built-in middlewares for logging and exception handling, simplifying their integration into your API.

### Logging Middleware
To log requests and responses, use the LoggingMiddleware. Register it as follows: 
```
var builder = WebApplication.CreateBuilder(args);
builder.AddLogging(LogLevel.Debug); // register console logging
...
var app = builder.Build();
...
app.UseMiddleware<LoggingMiddleware>(); // register middleware
...
app.Run();
```
By default, requests and responses are logged at the Information log level. For more control over logging, you can inherit from **LoggingMiddleware** and override its methods.

You can find more information about logging in the [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)

If you don't need the ability to edit your request and response logs, you can use the **built-in Microsoft middleware**:

```
var builder = WebApplication.CreateBuilder(args);
//...
builder.Services.AddHttpLogging(options => 
{
    options.LoggingFields = HttpLoggingFields.RequestHeaders |
                            HttpLoggingFields.RequestBody |
                            HttpLoggingFields.ResponseHeaders |
                            HttpLoggingFields.ResponseBody;
});
...
var app = builder.Build();
...
app.UseHttpLogging();
...
app.Run();
```
For more information, see [HTTP logging in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging/?view=aspnetcore-8.0)

### Exception Middleware
To catch and handle unexpected errors, use the **ExceptionMiddleware**. Register it like this:

```
var builder = WebApplication.CreateBuilder(args);
...
var app = builder.Build();
...
app.UseMiddleware<ExceptionMiddleware>(); // register middleware
...
app.Run();
```
Override the **ExtractFromException** and **CreateResponseJson** methods to customize the error response sent to clients.

## Versioning
**SmallApiToolkit** includes a **MapVersionGroup** extension method for versioning endpoint routes, ensuring your API can evolve without breaking existing clients.

```
...
var app = builder.Build();

...

app.MapGroup("someGroup")
  .MapVersionGroup(1)
  .MapGet("myGet", () => "This is a GET");

app.Run();
```

This creates the endpoint route: **someGroup/v1/myGet**. For more complex versioning needs, consider using: [https://github.com/dotnet/aspnet-api-versioning](https://github.com/dotnet/aspnet-api-versioning)

## Used in Examples
For a practical example of SmallApiToolkit in action, check out the [WeatherApi-VSA](https://github.com/Gramli/WeatherApi-VSA)repository.
