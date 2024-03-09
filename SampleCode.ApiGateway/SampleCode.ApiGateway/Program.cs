using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using SampleCode.ApiGateway.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

builder.Services.AddTransient<AddClientIpToRequestHeaderMiddleware>();

var app = builder.Build();

app.UseMiddleware<AddClientIpToRequestHeaderMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsPolicyBuilder => corsPolicyBuilder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseAuthorization();

app.Use(async (httpContext, next) =>
{
    if (httpContext.Request.Path == "/")
    {
        httpContext.Response.StatusCode = 200;
        await httpContext.Response.WriteAsync("Welcome to API Gateway");
    }
    else
    {
        await next(httpContext);
    }
});

await app.UseOcelot();

app.Run();
