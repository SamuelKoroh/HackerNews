using HackerNews.Application.Common.Interfaces;
using HackerNews.Application.Common.Models;
using HackerNews.Application.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;

namespace HackerNews.Application.UnitTests.Services;

public class StoryServiceTests
{
    [Fact]
    public async Task GetBestStoriesAsync_ReturnsCachedStories_WhenPresent()
    {
        // Arrange
        var cachedStories = new List<Story>
        {
            new() { Title = "Cached Story 1", Score = 50 },
            new() { Title = "Cached Story 2", Score = 30 }
        };

        var mockCache = new Mock<IMemoryCache>();
        var mockHackerNewsService = new Mock<IHackerNewsService>();
        var mockLogger = new Mock<ILogger<StoryService>>();

        object? outValue = cachedStories;
        mockCache
            .Setup(c => c.TryGetValue(It.IsAny<object>(), out outValue))
            .Returns(true);

        var service = new StoryService(mockHackerNewsService.Object, mockCache.Object, mockLogger.Object);

        // Act
        var result = await service.GetBestStoriesAsync(5);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Cached Story 1", result.First().Title);
        mockHackerNewsService.Verify(s => s.GetBestStoriesAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task GetBestStoriesAsync_FetchesFromServiceAndCaches_WhenNotInCache()
    {
        // Arrange
        var fetchedStories = new List<Story>
        {
            new() { Title = "Story 1", Score = 100 },
            new() { Title = "Story 2", Score = 75 }
        };

        var mockCache = new Mock<IMemoryCache>();
        var mockHackerNewsService = new Mock<IHackerNewsService>();
        var mockLogger = new Mock<ILogger<StoryService>>();

        object? unused;
        mockCache
            .Setup(c => c.TryGetValue(It.IsAny<object>(), out unused))
            .Returns(false);

        mockHackerNewsService
            .Setup(s => s.GetBestStoriesAsync(2))
            .ReturnsAsync(fetchedStories);

        var mockCacheSet = new Mock<ICacheEntry>();
        mockCache
            .Setup(c => c.CreateEntry(It.IsAny<object>()))
            .Returns(mockCacheSet.Object);

        var service = new StoryService(mockHackerNewsService.Object, mockCache.Object, mockLogger.Object);

        // Act
        var result = await service.GetBestStoriesAsync(2);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("Story 1", result.First().Title);
        mockHackerNewsService.Verify(s => s.GetBestStoriesAsync(2), Times.Once);
        mockCache.Verify(c => c.CreateEntry(It.IsAny<object>()), Times.Once);
    }
}