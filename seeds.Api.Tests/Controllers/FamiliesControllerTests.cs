using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class FamiliesControllerTests : ApiControllerTestsBase
{
    public List<Family> Families { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public int familiesIndexWithTags = 0;
    public FamiliesControllerTests()
        : base("api/Families/")
    {
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        Random random = new();
        for (int i = 0; i < 10; i++)
        {
            Families.Add(
            new Family()
            {
                CategoryKey = $"Cat #{i}?",
                Name = $"Category{i}"
            });
            Tags.Add(new()
            {
                Name = random.Next().ToString()
            });
        }
        Families[familiesIndexWithTags].Tags = Tags;
        if(!context.Family.Any()) { context.Family.AddRange(Families); }
        if(!context.Tag.Any()) { context.Tag.AddRange(Tags); }
    }
    
    [Fact]
    public async Task FamsController_GetAllEndpoint_ReturnsCorrectCount()
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
    public async Task FamsController_GetAllEndpoint_ReturnsFamilysTags()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<Family>>();
        result.Should().NotBeNull();
        result?.Should().Contain(f => f.Tags.Count == Tags.Count);
    }
    [Fact]
    public async Task FamsController_GetAllEndpoint_FamilyTagsAreOrderedByName()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<List<Family>>();
        var famWithTags = result.Should().Contain(f => f.Tags.Count == Tags.Count).Which;
        famWithTags.Should().NotBeNull();
        famWithTags.Tags.Should().BeInAscendingOrder(t => t.Name);
    }
    [Fact]
    public async Task FamsController_GetAllEndpoint_IfEmptyReturnsNotFound()
    {
        // Arrange
        context.Family.RemoveRange(Families);
        context.SaveChanges();
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
