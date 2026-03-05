using HackerNews.Application.Common.Interfaces;
using HackerNews.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HackerNews.Application.Extensions;

public static class ServicesCollelctionExtension
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IStoryService, StoryService>();

        return services;
    }
}
