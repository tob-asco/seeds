using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class TagsControllerTests : ApiControllerTestsBase
{
    public List<Category> Cats { get; set; } = new();
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
            Cats.Add(
            new Category()
            {
                Key = $"Cat #{i}",
            });
        }
        if(!_context.Category.Any()) { _context.Category.AddRange(Cats); }
        for (int i = 0; i <= 29; i++)
        {
            Tags.Add(
            new Tag()
            {
                CategoryKey = Cats[(int)i/10].Key,
                Name = $"tag #{i}"
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
        string catKey = Cats[0].Key;
        string name = Tags[0].Name;
        string url = $"api/Tags/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(name)}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<TagDto>();
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
