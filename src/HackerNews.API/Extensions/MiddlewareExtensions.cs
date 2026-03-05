using HackerNews.API.Middlewares;

namespace HackerNews.API.Extensions;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(
        this IApplicationBuilder app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
        return app;
    }
}