# In progress
# SmallApiToolkit

## RequestHandler

## Midlewares

### Logging Middleware

### Exception Middleware

## Versioning
You can simply use MapVersionGroup extension method like this:

```
...
var app = builder.Build();

...

app.MapGroup("someGroup")
  .MapVersionGroup(1)
  .MapGet("myGet", () => "This is a GET");

app.Run();
```

It creates endpoint route: **someGroup/v1/myGet**

If you need more complex versioning use: [https://github.com/dotnet/aspnet-api-versioning](https://github.com/dotnet/aspnet-api-versioning)

## Used in Examples
