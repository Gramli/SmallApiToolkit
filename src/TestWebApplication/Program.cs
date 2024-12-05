using Microsoft.AspNetCore.Mvc;
using SmallApiToolkit.Extensions;
using SmallApiToolkit.Middleware;
using TestWebApplication.WeatherForecast;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();

builder.Services.ConfigureWeatherForecast();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app
    .MapGroup("weather")
    .MapVersionGroup(1)
    .AddForecastEndpoints();

app.Run();
