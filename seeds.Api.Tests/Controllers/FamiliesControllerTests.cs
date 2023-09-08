using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class FamiliesControllerTests : ApiControllerTestsBase
{
    public List<Family> Families { get; set; } = new();
    public List<Topic> Topics { get; set; } = new();
    public int familiesIndexWithTopics = 0;
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
            Topics.Add(new()
            {
                Name = random.Next().ToString()
            });
        }
        Families[familiesIndexWithTopics].Topics = Topics;
        if(!context.Family.Any()) { context.Family.AddRange(Families); }
        if(!context.Topic.Any()) { context.Topic.AddRange(Topics); }
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
    public async Task FamsController_GetAllEndpoint_ReturnsFamilysTopics()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<Family>>();
        result.Should().NotBeNull();
        result?.Should().Contain(f => f.Topics.Count == Topics.Count);
    }
    [Fact]
    public async Task FamsController_GetAllEndpoint_FamilyTopicsAreOrderedByName()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<List<Family>>();
        var famWithTopics = result.Should().Contain(f => f.Topics.Count == Topics.Count).Which;
        famWithTopics.Should().NotBeNull();
        famWithTopics.Topics.Should().BeInAscendingOrder(t => t.Name);
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
