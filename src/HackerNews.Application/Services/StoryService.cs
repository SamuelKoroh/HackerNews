using HackerNews.Application.Common.Interfaces;
using HackerNews.Application.Common.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace HackerNews.Application.Services;

public class StoryService(IHackerNewsService hackerNewsService, IMemoryCache cache, ILogger<StoryService> logger) : IStoryService
{
    public async Task<IReadOnlyCollection<Story>> GetBestStoriesAsync(int n)
    {
        string cacheKey = $"best_stories-{n}";
        if (cache.TryGetValue(cacheKey, out List<Story>? cached))
        {
            logger.LogInformation("Returning {Count} stories from cache", cached!.Count);
            return cached;
        }

        var stories = await hackerNewsService.GetBestStoriesAsync(n);
        logger.LogInformation("Fetched {Count} stories from Hacker News API", stories.Count);

        cache.Set(cacheKey, stories, TimeSpan.FromMinutes(5));

        return stories;
    }
}
