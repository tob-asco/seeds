﻿using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using seeds.Api.Data;

namespace seeds.Api.Tests.Controllers;

public class ApiControllerTestsBase : IDisposable
{
    protected readonly seedsApiContext context;
    protected readonly HttpClient _httpClient;
    protected readonly string baseUri = "";
    public ApiControllerTestsBase(string baseUri)
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        context = new seedsApiContext(options);
        context.Database.EnsureCreated();

        // Create the HttpClient using the in-memory server
        var factory = new WebApplicationFactory<ProgramTest>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(context);
                });
            });
        _httpClient = factory.CreateClient();
        this.baseUri = baseUri;
    }

    // Disposing the context is important to avoid "already tracked" errors
    public void Dispose()
    {
        context.Database.EnsureDeleted();
        context.Dispose();
        _httpClient.Dispose();
    }
}
