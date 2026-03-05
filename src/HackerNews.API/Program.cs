using Asp.Versioning;
using HackerNews.API.Extensions;
using HackerNews.Application.Extensions;
using HackerNews.Infrastructure.Extensions;
using Serilog;

namespace HackerNews.API;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();
            builder.Host.UseSerilog();

            // Add services to the container.
            builder.Services.AddInfrastructure();
            builder.Services.AddApplication();
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();

            //builder.Services.Configure< HackerNewsApiOption>();
            builder.Services.AddApiClients(builder.Configuration);

            // API Versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            builder.Services.AddRateLimiterConfiguration();
            builder.Services.AddProblemDetails();

            // Swagger
            builder.Services.AddSwaggerDocumentation();


            var app = builder.Build();
            app.UseGlobalExceptionHandler();

            if (!app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "HackerNews API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
