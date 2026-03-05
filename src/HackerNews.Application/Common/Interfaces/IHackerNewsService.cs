using HackerNews.Application.Common.Models;

namespace HackerNews.Application.Common.Interfaces;

public interface IHackerNewsService
{
    Task<IReadOnlyCollection<Story>> GetBestStoriesAsync(int limit);
}