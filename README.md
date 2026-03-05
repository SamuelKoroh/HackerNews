## HackerNews API
ASP.NET Core Web API to retrieve the top N stories from Hacker News, ordered by score. The API uses Refit, caching, Polly resilience, rate limiting, and global exception handling.


## Getting Started
1. Clone the repository:

```
git clone https://github.com/<yourusername>/HackerNews.API.git
cd HackerNews.API
```

2. Install dependencies:

 ```dotnet restore```

3. Run the API:
```bash
dotnet run
```
4. Access Swagger UI:

Open your browser:

``` 
https://localhost:7166/swagger
```

## Running Tests
Unit test projects are located in the Tests folder.
## Run All Tests
```bash 
dotnet test
```
Run Tests for a Specific Project

```bash
dotnet test HackerNews.Application.UnitTests
dotnet test HackerNews.Infrastructure.UnitTests
```

## Test Coverage
Basic tests were written to cover
- StoryService
- Caching behavior
- External API abstraction


## Features

- Retrieves the best stories from Hacker News API.
- Memory caching for 5 minutes to reduce external API calls.
- Refit typed REST client for external API calls.
- Polly retry policies for transient HTTP errors.
- Global exception handling for all Refit and HTTP errors.
- Structured logging via ILogger/Serilog.
- API versioning for future expansion.
- Rate limiting to protect both your API and Hacker News.
- Swagger UI for interactive API documentation.


## Technologies and Tools
- ASP.NET Core 8.0 Web API
- Refit for typed HTTP client
- Polly for resilience and retries
- MemoryCache for caching requests
- Serilog for logging
- Swashbuckle for Swagger UI
- API Versioning via Asp.Versioning.Mvc

## API Endpoint
```http
GET /api/v1/stories/best?n={number}
```
- n – Number of top stories to retrieve (default = 10).

### Response:
```JSON
[
  {
    "title": "A uBlock Origin update was rejected from the Chrome Web Store",
    "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
    "postedBy": "ismaildonmez",
    "time": "2019-10-12T13:43:01Z",
    "score": 1716,
    "commentCount": 572,
    ...
  },
  ...
]
```

## Future Improvements

- Redis Distributed Cache for horizontal scaling.
- Background HostedService to refresh cached stories periodically.
- Circuit breaker policy with Polly.
- Metrics & Monitoring.
- More Unit and integration tests with coverage for retry, and exception handling.