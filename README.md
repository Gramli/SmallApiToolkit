# In progress
# SmallApiToolkit
SmallApiToolkit offers a range of features, including **ExceptionMiddleware**, which handles unexpected errors gracefully, **LoggingMiddleware** for comprehensive request and response logging, the **MapVersionGroup** extension method to streamline API versioning, and the **IHttpRequestHandler** interface, which simplifies the creation of consistent JSON responses.

SmallApiToolkit is designed to help developers build small APIs faster and more efficiently.

## RequestHandler
To ensure Minimal API endpoints are clear and readable, **SmallApiToolkit** provides the IHttpRequestHandler interface. If your handler implements the **IHttpRequestHandler** interface, you can simply call the **SendAsync** extension method in the Minimal API endpoint with the request and CancellationToken as parameters. The **SendAsync** extension method automatically creates a JSON response based on the implementation of **IHttpRequestHandler**. Additionally, every response is wrapped in a DataResponse, which unifies all responses into the same structure, containing **Data** and **Errors**.

```
public class DataResponse<T>
{
    public T? Data { get; init; }

    public IEnumerable<string> Errors { get; init; } = Array.Empty<string>();

}
```

Example IHttpRequestHandler usage:
* First implement IHttpRequestHandler interface

```
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

* Register Handler in DI container:

```
serviceCollection.AddScoped<IHttpRequestHandler<int, AddFavoriteCommand>, AddFavoriteHandler>()
```
* Register Minimal Api endpoint:

```
MapPost("favorite", async ([FromBody] AddFavoriteCommand addFavoriteCommand, [FromServices] IHttpRequestHandler<int, AddFavoriteCommand> handler, CancellationToken cancellationToken) =>
                    await handler.SendAsync(addFavoriteCommand, cancellationToken))
                        .ProducesDataResponse<int>()
                        .WithName("AddFavorite")
                        .WithTags("Setters");
```


## Midlewares
Middlewares in web APIs are essential for handling cross-cutting concerns such as authentication, logging, and error handling by processing requests and responses in a modular and reusable way. 

### Logging Middleware
To log requests and responses, you can use **LoggingMiddleware**. To use it, you need to register logging, which means adding a logging provider and then registering the middleware. **SmallApiToolkit** allows you to register simple console logging using the **AddLogging** extension method. You can find more information about logging in the [Microsoft documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-8.0)
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
When you register **LoggingMiddleware**, all requests and responses will be logged. By default, requests and responses are logged using the Information log level. If you need to change the log structure or level, you can inherit from **LoggingMiddleware** and override the **LogRequest** and **LogResponse** methods.

### Exception Middleware
To catch unexpected errors, we often use exception middleware. **SmallApiToolkit** allows you to register the **ExceptionMiddleware** class, which catches exceptions on every request. You can register it like this:

```
var builder = WebApplication.CreateBuilder(args);
...
var app = builder.Build();
...
app.UseMiddleware<ExceptionMiddleware>(); // register middleware
...
app.Run();
```
When you register the **ExceptionMiddleware**, all exceptions will be caught (unless they are handled lower in the code). If you need to change the response sent to the client after an exception is caught, you can override the **ExtractFromException** method, which creates the response status and message. Additionally, if you need to define a custom JSON object that is sent to the client, you can override the **CreateResponseJson** method. Both methods are called from the **WriteResponseAsync** method, which you can also override. The **WriteResponseAsync** method takes the parameters of Exception and HttpContext.

## Versioning
**SmallApiToolkit** has a **MapVersionGroup** extension method that adds a version to an endpoint route. You can use **MapVersionGroup** like this:

```
...
var app = builder.Build();

...

app.MapGroup("someGroup")
  .MapVersionGroup(1)
  .MapGet("myGet", () => "This is a GET");

app.Run();
```

This creates the endpoint route: **someGroup/v1/myGet**

If you need more complex versioning, use: [https://github.com/dotnet/aspnet-api-versioning](https://github.com/dotnet/aspnet-api-versioning)

## Used in Examples
