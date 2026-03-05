using System.Net;
using System.Text.Json;
using Refit;

namespace HackerNews.API.Middlewares;

public class GlobalExceptionMiddleware(RequestDelegate next,
        ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ApiException apiEx)
        {
            logger.LogError(apiEx, "Refit API error: {StatusCode}", apiEx.StatusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)apiEx.StatusCode;

            var response = new
            {
                title = "External API Error",
                status = (int)apiEx.StatusCode,
                detail = apiEx.Content ?? apiEx.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx, "HTTP request failed");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;

            var response = new
            {
                title = "External Service Unavailable",
                status = (int)HttpStatusCode.ServiceUnavailable,
                detail = httpEx.Message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                title = "Internal Server Error",
                status = 500,
                detail = ex.Message
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}