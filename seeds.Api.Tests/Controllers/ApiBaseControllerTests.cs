using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class ApiBaseControllerTests : IDisposable
{
    protected readonly seedsApiContext _context;
    protected readonly CategoryUserPreferencesController _controller;
    protected readonly HttpClient _httpClient;

    public ApiBaseControllerTests()
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new seedsApiContext(options);
        _context.Database.EnsureCreated();
        _controller = new CategoryUserPreferencesController(_context);

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
