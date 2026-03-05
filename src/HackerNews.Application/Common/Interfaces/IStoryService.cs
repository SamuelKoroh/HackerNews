using HackerNews.Application.Common.Models;

namespace HackerNews.Application.Common.Interfaces;

public interface IStoryService
{
    Task<IReadOnlyCollection<Story>> GetBestStoriesAsync(int n);
}
