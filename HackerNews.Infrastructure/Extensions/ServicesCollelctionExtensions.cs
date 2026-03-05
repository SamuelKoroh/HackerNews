using HackerNews.Application.Common.Interfaces;
using HackerNews.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Infrastructure.Extensions;

public static class ServicesCollelctionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IHackerNewsService, HackerNewsService>();

        return services;
    }
}
