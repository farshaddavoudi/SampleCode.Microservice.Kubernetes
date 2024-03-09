using System.Net;

namespace SampleCode.ApiGateway.Middlewares;

public class AddClientIpToRequestHeaderMiddleware : IMiddleware
{

    private const string ClientIpCustomHeaderName = "X-Client-IP";

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var clientIp = GetClientIp(context);

        if (string.IsNullOrWhiteSpace(clientIp))
        {
            await ReturnErrorResponse(context);
        }
        else
        {
            context.Request.Headers.TryAdd(ClientIpCustomHeaderName, clientIp);

            await next(context);
        }
    }

    private string? GetClientIp(HttpContext context)
    {
        // Get external network call's IP address [Behind WAF]
        //Internal network
        // Get internal network call's IP address
        var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                 ?? context.Connection.RemoteIpAddress?.ToString();

        return ip;
    }

    private async Task ReturnErrorResponse(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        await context.Response.WriteAsync("IP address cannot be detected");
        await context.Response.StartAsync();
    }

}