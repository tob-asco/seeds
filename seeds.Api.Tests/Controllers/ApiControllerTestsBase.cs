using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using seeds.Api.Data;

namespace seeds.Api.Tests.Controllers;

public class ApiControllerTestsBase : IDisposable
{
    protected readonly seedsApiContext _context;
    protected readonly HttpClient _httpClient;
    public ApiControllerTestsBase()
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new seedsApiContext(options);
        _context.Database.EnsureCreated();

        // Create the HttpClient using the in-memory server
        var factory = new WebApplicationFactory<ProgramTest>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_context);
                });
            });
        _httpClient = factory.CreateClient();
    }

    // Disposing the context is important to avoid "already tracked" errors
    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        _httpClient.Dispose();
    }
}
