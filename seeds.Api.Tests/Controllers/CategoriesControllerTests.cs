using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class CategoriesControllerTests : ApiControllerTestsBase
{
    public List<Category> Categories { get; set; } = new();

    public CategoriesControllerTests()
        :base(baseUri: "api/Categories/")
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
                Key = $"Cat #{i}?",
                Name = $"Category{i}"
            });
        }
        if(!_context.Category.Any()) { _context.Category.AddRange(Categories); }
    }

    [Fact]
    public async Task CatsController_GetAllEndpoint_ReturnsListOfCorrectLength()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<CategoryDto>>();
        result.Should().NotBeNull();
        result?.Should().HaveCount(Categories.Count);
    }
    [Fact]
    public async Task CatsController_GetAllEndpoint_IfEmptyReturnsNotFound()
    {
        // Arrange
        _context.Category.RemoveRange(Categories);
        _context.SaveChanges();
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CatsController_GetEndpoint_ReturnsCat()
    {
        // Arrange
        string key = Categories[0].Key;
        string url = baseUri + $"{HttpUtility.UrlEncode(key)}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<CategoryDto>();
        result.Should().NotBeNull();
        result?.Key.Should().Be(key);
    }
    [Fact]
    public async Task CatsController_GetEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        string url = baseUri + $"ThisShouldBeNoValidKey";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
