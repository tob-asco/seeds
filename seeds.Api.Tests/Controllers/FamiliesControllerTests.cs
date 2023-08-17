﻿using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class FamiliesControllerTests : ApiControllerTestsBase
{
    public List<Family> Families { get; set; } = new();
    public FamiliesControllerTests()
        : base("api/Families/")
    {
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Families.Add(
            new Family()
            {
                CategoryKey = $"Cat #{i}?",
                Name = $"Category{i}"
            });
        }
        if(!_context.Family.Any()) { _context.Family.AddRange(Families); }
    }
    
    [Fact]
    public async Task FamsController_GetAllEndpoint_ReturnsItselfs()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<Family>>();
        result.Should().NotBeNull();
        result?.Should().HaveCount(Families.Count);
    }
    [Fact]
    public async Task FamsController_GetAllEndpoint_IfEmptyReturnsNotFound()
    {
        // Arrange
        _context.Family.RemoveRange(Families);
        _context.SaveChanges();
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}