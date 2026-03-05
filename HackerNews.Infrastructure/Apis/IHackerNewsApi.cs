using HackerNews.Infrastructure.Responses;
using Refit;

namespace HackerNews.Infrastructure.Apis;

public interface IHackerNewsApi
{
    [Get("/v0/beststories.json")]
    Task<List<int>> GetBestStoryIds();

    [Get("/v0/item/{id}.json")]
    Task<HackerNewsItemResponse?> GetItem(int id);
}