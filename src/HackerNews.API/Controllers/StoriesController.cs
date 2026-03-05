using Asp.Versioning;
using HackerNews.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace HackerNews.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/stories")]
public class StoriesController(IStoryService storyService) : ControllerBase
{
    [HttpGet("best")]
    [EnableRateLimiting("api")]

    public async Task<IActionResult> GetBestStories([FromQuery] int n = 10)
    {
        if (n <= 0)
            return BadRequest("n must be greater than zero");

        var stories = await storyService.GetBestStoriesAsync(n);

        return Ok(stories);
    }
}