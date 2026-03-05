using HackerNews.API.Options;
using HackerNews.Infrastructure.Apis;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace HackerNews.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<HackerNewsApiOptions>(o => configuration.GetSection(HackerNewsApiOptions.Section).Bind(o));
        services.AddRefitClient<IHackerNewsApi>()
            .ConfigureHttpClient((sp, client) =>
            {
                var options = sp
                    .GetRequiredService<IOptions<HackerNewsApiOptions>>()
                    .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
            })
            .AddPolicyHandler(GetRetryPolicy());


        return services;
    }

    public static IServiceCollection AddSwaggerDocumentation(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "HackerNews API",
                Version = "v1",
                Description = "Retrieve top HackerNews stories"
            });
        });

        return services;
    }

    public static IServiceCollection AddRateLimiterConfiguration(
    this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("api", opt =>
            {
                opt.PermitLimit = 100;
                opt.Window = TimeSpan.FromMinutes(1);
                opt.QueueLimit = 10;
            });
        });

        return services;
    }

    private static AsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                3,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
    }
}
