using HackerNews.Infrastructure.Apis;
using HackerNews.Infrastructure.Responses;
using HackerNews.Infrastructure.Services;
using Moq;

namespace HackerNews.Infrastructure.UnitTests.Services;

public class HackerNewsServiceTests
{
    [Fact]
    public async Task GetBestStoriesAsync_ReturnsStoriesOrderedByScore_LimitedByParameter()
    {
        // Arrange
        var mockApi = new Mock<IHackerNewsApi>();


        mockApi.Setup(api => api.GetBestStoryIds())
            .ReturnsAsync([1, 2, 3]);

        mockApi.Setup(api => api.GetItem(1)).ReturnsAsync(new HackerNewsItemResponse
        {
            Id = 1,
            Title = "Story 1",
            Url = "http://story1.com",
            By = "Author1",
            Time = 1000000,
            Score = 10,
            Descendants = 5
        });

        mockApi.Setup(api => api.GetItem(2)).ReturnsAsync(new HackerNewsItemResponse
        {
            Id = 2,
            Title = "Story 2",
            Url = "http://story2.com",
            By = "Author2",
            Time = 2000000,
            Score = 30,
            Descendants = 10
        });

        mockApi.Setup(api => api.GetItem(3)).ReturnsAsync(new HackerNewsItemResponse
        {
            Id = 3,
            Title = "Story 3",
            Url = "http://story3.com",
            By = "Author3",
            Time = 3000000,
            Score = 20,
            Descendants = 7
        });

        var service = new HackerNewsService(mockApi.Object);

        int limit = 2;

        // Act
        var result = await service.GetBestStoriesAsync(limit);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(limit, result.Count);
        Assert.Equal(30, result.First().Score);
        Assert.Equal("Story 2", result.First().Title);
        Assert.Equal("Story 3", result.Last().Title);
    }

    [Fact]
    public async Task GetBestStoriesAsync_HandlesNullItemsGracefully()
    {
        // Arrange
        var mockApi = new Mock<IHackerNewsApi>();
        mockApi.Setup(api => api.GetBestStoryIds())
            .ReturnsAsync([1, 2]);

        mockApi.Setup(api => api.GetItem(1))
            .ReturnsAsync((HackerNewsItemResponse?)null);

        mockApi.Setup(api => api.GetItem(2))
            .ReturnsAsync(new HackerNewsItemResponse
            {
                Id = 2,
                Title = "Story 2",
                Url = "http://story2.com",
                By = "Author2",
                Time = 2000000,
                Score = 30,
                Descendants = 10
            });

        var service = new HackerNewsService(mockApi.Object);

        // Act
        var result = await service.GetBestStoriesAsync(5);

        // Assert
        Assert.Single(result);
        Assert.Equal("Story 2", result.First().Title);
    }
}