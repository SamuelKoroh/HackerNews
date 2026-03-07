using HackerNews.Application.Common.Interfaces;
using HackerNews.Application.Common.Models;
using HackerNews.Infrastructure.Apis;

namespace HackerNews.Infrastructure.Services;

public class HackerNewsService(IHackerNewsApi api) : IHackerNewsService
{
    public async Task<IReadOnlyCollection<Story>> GetBestStoriesAsync(int limit)
    {
        var ids = await api.GetBestStoryIds();

        var tasks = ids
            .Take(limit)
            .Select(api.GetItem);

        var items = await Task.WhenAll(tasks);

        var stories = items
            .Where(x => x != null)
            .Select(x => new Story
            {
                Title = x!.Title ?? "",
                Uri = x.Url ?? "",
                PostedBy = x.By ?? "",
                Time = DateTimeOffset
                    .FromUnixTimeSeconds(x.Time)
                    .UtcDateTime,
                Score = x.Score,
                CommentCount = x.Descendants,
                Id = x.Id,
                Kids = x.Kids,
                Type = x.Type,
            })
            .OrderByDescending(x => x.Score)
            .ToList();

        return stories;
    }
}
