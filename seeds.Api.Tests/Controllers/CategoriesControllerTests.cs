using FakeItEasy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Dto.ToApi;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class CategoriesControllerTests : ApiBaseControllerTests
{
    public List<Category> Categories { get; set; } = new();

    public CategoriesControllerTests()
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
            Categories.Add(
            new Category()
            {
                Key = $"Cat{i}",
                Name = $"Category{i}"
            });
        }
        if(!_context.Category.Any()) { _context.Category.AddRange(Categories); }
    }

    [Fact]
    public async Task CatsController_GetAllEndpoint_ReturnsListOfCorrectLength()
    {
        // Arrange
        string url = $"api/Categories";

        // Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<List<CategoryDtoApi>>();

        // Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Should().HaveCount(Categories.Count);
    }
    [Fact]
    public async Task CatsController_GetEndpoint_ReturnsCat()
    {
        // Arrange
        string key = Categories[0].Key;
        string url = $"api/Categories/{key}";

        // Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<CategoryDtoApi>();

        // Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Key.Should().Be(key);
    }
    [Fact]
    public async Task CatsController_GetEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        string url = $"api/Categories/ThisShouldBeNoValidKey";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
