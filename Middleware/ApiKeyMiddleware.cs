using AuthService.Attributes;
using AuthService.Data;
using Microsoft.AspNetCore.Http.Features;

namespace AuthService.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AuthContext authContext)
    {
        var endpoint = context.Features.Get<IEndpointFeature>()?.Endpoint;
        var attribute = endpoint?.Metadata.GetMetadata<RequireApiKeyAttribute>();
        if (attribute != null)
        {
            var authHeader = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ");
            var key = authHeader?[^1];

            if (string.IsNullOrEmpty(key))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Necesita un api key para acceder");
                return;
            }

            var apiKey = authContext.ApiKeys.FirstOrDefault(apiKey => apiKey.Key == key);

            if (apiKey is null)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Api key inválido");
                return;
            }
        }

        await _next(context);
    }
}
