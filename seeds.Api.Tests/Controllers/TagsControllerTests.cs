using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class TagsControllerTests : ApiBaseControllerTests
{
    public List<Category> Categories { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();

    public TagsControllerTests()
    {
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 0; i <= 2; i++)
        {
            Categories.Add(
            new Category()
            {
                Key = $"Cat{i}",
            });
        }
        if(!_context.Category.Any()) { _context.Category.AddRange(Categories); }
        for (int i = 0; i <= 29; i++)
        {
            Tags.Add(
            new Tag()
            {
                CategoryKey = $"Cat{(int)i/10}",
                Name = $"tag{i}"
            });
        }
        if(!_context.Tag.Any()) { _context.Tag.AddRange(Tags); }
    }

    [Fact]
    public async Task TagsController_GetAllEndpoint_ReturnsListOfCorrectLength()
    {
        // Arrange
        string url = $"api/Tags";

        // Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<List<TagDto>>();

        // Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Should().HaveCount(Tags.Count);
    }
    [Fact]
    public async Task TagsController_GetAllEndpoint_IfEmptyReturnsNotFound()
    {
        // Arrange
        _context.Tag.RemoveRange(Tags);
        _context.SaveChanges();
        string url = $"api/Tags";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task TagsController_GetEndpoint_ReturnsTag()
    {
        // Arrange
        string catKey = Categories[0].Key;
        string name = Tags[0].Name;
        string url = $"api/Tags/{catKey}/{name}";

        // Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<TagDto>();

        // Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(catKey);
        result?.Name.Should().Be(name);
    }
    [Fact]
    public async Task TagsController_GetEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        string url = $"api/Tags/ThisShouldBeNoValidKey/ok";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

}
