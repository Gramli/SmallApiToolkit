# SmallApiToolkit

[![NuGet version](https://img.shields.io/nuget/v/SmallApiToolkit?style=flat-square)](https://www.nuget.org/packages/SmallApiToolkit)
[![Build Status](https://img.shields.io/github/actions/workflow/status/Gramli/SmallApiToolkit/build.yml?style=flat-square)](https://github.com/Gramli/SmallApiToolkit/actions)
[![.NET](https://img.shields.io/badge/.NET-10.0-512bd4?style=flat-square)](https://dotnet.microsoft.com)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)](LICENSE.md)

A C# library designed for building small-scale or example web APIs quickly and efficiently. Streamline your development process with ready-to-use extensions, middleware, and support classes for ASP.NET Core Minimal APIs.

[Features](#features) • [Getting Started](#getting-started) • [Documentation](#documentation) • [Examples](#examples)

## Why SmallApiToolkit?

Many small or educational projects require similar abstractions, extensions, and middleware for tasks like exception handling, logging, and request validation. **SmallApiToolkit** eliminates this boilerplate code, allowing you to focus on building your API's core functionality.

The `IHttpRequestHandler` abstraction and the `SendAsync` extension method are production-ready patterns that provide:
- **Clean, maintainable endpoints** with clear separation of concerns
- **Consistent response structure** with built-in error handling
- **Reduced boilerplate** through reusable components
- **Validation-first approach** to ensure data integrity

## Features

- **Request Handler Pattern**: Clean abstraction for endpoint logic with built-in validation support
- **Caching Support**: Both HTTP response caching headers and ASP.NET Core output caching with custom policies
- **Middleware Ready**: Pre-built logging and exception handling middleware
- **API Versioning**: Simple route-based versioning for Minimal APIs
- **CORS Configuration**: Quick setup via configuration files
- **Consistent Responses**: Standardized `DataResponse<T>` wrapper for all API responses

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) or later

### Installation

Install the NuGet package:

```bash
dotnet add package SmallApiToolkit
```

Or via Package Manager Console:

```powershell
Install-Package SmallApiToolkit
```

### Quick Start

1. **Create a request handler**:

```csharp
public class GetWeatherHandler : IHttpRequestHandler<WeatherForecastDto[], EmptyRequest>
{
    public Task<HttpDataResponse<WeatherForecastDto[]>> HandleAsync(
        EmptyRequest request, 
        CancellationToken cancellationToken)
    {
        var forecast = GenerateWeatherForecast();
        return Task.FromResult(HttpDataResponses.AsOK(forecast));
    }
}
```

2. **Register in DI container**:

```csharp
builder.Services.AddScoped<IHttpRequestHandler<WeatherForecastDto[], EmptyRequest>, GetWeatherHandler>();
```

3. **Create Minimal API endpoint**:

```csharp
app.MapGet("/weather", 
    async (IHttpRequestHandler<WeatherForecastDto[], EmptyRequest> handler, CancellationToken ct) =>
        await handler.SendAsync(EmptyRequest.Instance, ct))
    .ProducesDataResponse<WeatherForecastDto[]>();
```

That's it! Your endpoint now returns a consistent `DataResponse<T>` structure with built-in error handling.

## Documentation

### Request Handlers

The `IHttpRequestHandler<TResponse, TRequest>` interface provides a clean abstraction for endpoint logic. All responses are wrapped in a `DataResponse<T>` structure:

```csharp
public class DataResponse<T>
{
    public T? Data { get; init; }
    public IEnumerable<string> Errors { get; init; } = Array.Empty<string>();
}
```

#### Validation Handler

For endpoints requiring validation, use `ValidationHttpRequestHandler<TResponse, TRequest>`:

```csharp
public class CreateUserHandler : ValidationHttpRequestHandler<UserDto, CreateUserRequest>
{
    public CreateUserHandler(IRequestValidator<CreateUserRequest> validator) 
        : base(validator) { }

    protected override Task<HttpDataResponse<UserDto>> HandleValidRequestAsync(
        CreateUserRequest request, 
        CancellationToken cancellationToken)
    {
        // Your logic here - validation is already done
        var user = CreateUser(request);
        return Task.FromResult(HttpDataResponses.AsOK(user));
    }
}
```

Implement the validator:

```csharp
public class CreateUserValidator : IRequestValidator<CreateUserRequest>
{
    public RequestValidationResult Validate(CreateUserRequest request)
    {
        if (string.IsNullOrEmpty(request.Email))
            return new RequestValidationResult { IsValid = false };
            
        return new RequestValidationResult { IsValid = true };
    }
}
```

Register both in DI:

```csharp
builder.Services
    .AddScoped<IHttpRequestHandler<UserDto, CreateUserRequest>, CreateUserHandler>()
    .AddScoped<IRequestValidator<CreateUserRequest>, CreateUserValidator>();
```

### Caching

SmallApiToolkit provides two complementary caching approaches:

#### HTTP Response Caching Headers

Adds `Cache-Control` headers for client-side and proxy caching:

```csharp
app.MapGet("/weather", 
    async (IHttpRequestHandler<WeatherForecastDto[], EmptyRequest> handler, CancellationToken ct) =>
        await handler.SendAsync(EmptyRequest.Instance, ct))
    .AddResponseCacheHeader(300); // Cache for 5 minutes
```

**Features**:
- Only applies to GET/HEAD requests returning 200 OK
- Automatically skips error responses
- No server-side resource requirements

#### Output Caching (Server-Side)

Uses ASP.NET Core's output caching with intelligent custom policies.

**Basic Setup**:

1. Register custom policies:

```csharp
builder.Services.AddOutputCacheWithCustomPolicy(
    ("weather", TimeSpan.FromMinutes(5)),
    ("short", TimeSpan.FromSeconds(30))
);
```

2. Enable middleware:

```csharp
app.UseOutputCache();
```

3. Apply to endpoints:

```csharp
app.MapGet("/weather", 
    async (IHttpRequestHandler<WeatherForecastDto[], EmptyRequest> handler, CancellationToken ct) =>
        await handler.SendAsync(EmptyRequest.Instance, ct))
    .CacheOutput("weather");
```

**Advanced Setup with Additional Policies**:

You can combine custom policies with ASP.NET Core's built-in policies:

```csharp
builder.Services.AddOutputCacheWithCustomPolicy(
    configureOptions: options =>
    {
        // Add built-in base policy
        options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10)));
        
        // Add custom named policy with specific rules
        options.AddPolicy("myPolicy", builder => 
            builder
                .Expire(TimeSpan.FromMinutes(1))
                .SetVaryByQuery("id", "type"));
    },
    // Your custom policies
    ("weather", TimeSpan.FromMinutes(5)),
    ("short", TimeSpan.FromSeconds(30))
);
```

Or use only custom policies without additional configuration:

```csharp
// Just custom policies
builder.Services.AddOutputCacheWithCustomPolicy(
    ("weather", TimeSpan.FromMinutes(5))
);

// With additional ASP.NET Core policies
builder.Services.AddOutputCacheWithCustomPolicy(
    options => options.AddBasePolicy(builder => builder.Expire(TimeSpan.FromSeconds(10))),
    ("weather", TimeSpan.FromMinutes(5))
);
```

**Custom Policy Rules**:
- Only caches GET/HEAD requests
- Excludes responses with `Set-Cookie` headers
- Only caches 200 OK and 301 responses
- Varies by query string parameters

**Comparison**:

| Feature | AddResponseCacheHeader | Output Caching |
|---------|------------------------|----------------|
| **Location** | Client & proxies | Server memory |
| **Setup** | Single line | Requires registration |
| **Performance** | Reduces bandwidth | Prevents execution |
| **Best For** | Public APIs | High-traffic endpoints |

### Middleware

#### Logging Middleware

Logs all requests and responses:

```csharp
builder.Services.AddLogging(options => options.SetMinimumLevel(LogLevel.Information));

var app = builder.Build();
app.UseMiddleware<LoggingMiddleware>();
```

Customize by inheriting from `LoggingMiddleware` and overriding its methods.

> **Note**: For simpler scenarios, consider using [ASP.NET Core's built-in HTTP logging](https://learn.microsoft.com/aspnet/core/fundamentals/http-logging).

#### Exception Middleware

Catches unhandled exceptions and returns consistent error responses:

```csharp
var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
```

Customize error responses by overriding `ExtractFromException` and `CreateResponseJson` methods.

### API Versioning

Simple route-based versioning for Minimal APIs:

```csharp
app.MapGroup("api")
    .MapVersionGroup(1)
    .MapGet("users", GetUsers);

// Creates route: api/v1/users
```

For more advanced versioning needs, see [asp-net-api-versioning](https://github.com/dotnet/aspnet-api-versioning).

### CORS Configuration

Configure CORS via `appsettings.json`:

```json
{
  "CORS": {
    "name": "allowLocalhost",
    "urls": [
      "http://localhost:4200",
      "https://localhost:4200"
    ]
  }
}
```

Register in `Program.cs`:

```csharp
var corsPolicyName = builder.Services.AddCorsByConfiguration(builder.Configuration);

var app = builder.Build();
app.UseCors(corsPolicyName);
```

This creates a policy allowing the specified origins with any headers and methods. Omitting URLs allows any origin.

## Examples

See SmallApiToolkit in action in these projects:

- [WeatherApi-VSA](https://github.com/Gramli/WeatherApi-VSA) - Vertical Slice Architecture example
- [AuthApi](https://github.com/Gramli/AuthApi) - Authentication and authorization
- [FileApi](https://github.com/Gramli/FileApi) - File upload and management

## Resources

- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis)
- [Output Caching in ASP.NET Core](https://learn.microsoft.com/aspnet/core/performance/caching/output)
- [Response Caching](https://learn.microsoft.com/aspnet/core/performance/caching/response)
- [HTTP Logging](https://learn.microsoft.com/aspnet/core/fundamentals/http-logging)

## Motivation

This library was created to reduce repetitive code across my example and educational projects. While designed for quick prototyping and small APIs, the core patterns (especially `IHttpRequestHandler`) are production-ready and actively used in real applications.

Feel free to adopt these ideas in your own projects!
