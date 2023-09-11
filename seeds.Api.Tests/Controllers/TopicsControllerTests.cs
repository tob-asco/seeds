using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class TopicsControllerTests : ApiControllerTestsBase
{
    public List<Category> Cats { get; set; } = new();
    public List<Topic> Topics { get; set; } = new();

    public TopicsControllerTests()
        :base(baseUri: "api/Topics/")
    {
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        context.ChangeTracker.Clear();
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
        if(!context.Category.Any()) { context.Category.AddRange(Cats); }
        for (int i = 0; i <= 29; i++)
        {
            Topics.Add(
            new Topic()
            {
                CategoryKey = Cats[(int)i/10].Key,
                Name = $"topic #{i}"
            });
        }
        if(!context.Topic.Any()) { context.Topic.AddRange(Topics); }
    }

    [Fact]
    public async Task TopicsController_GetAllEndpoint_ReturnsListOfCorrectLength()
    {
        // Arrange
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<List<TopicFromDb>>();

        // Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Should().HaveCount(Topics.Count);
    }
    [Fact]
    public async Task TopicsController_GetAllEndpoint_IfEmptyReturnsNotFound()
    {
        // Arrange
        context.Topic.RemoveRange(Topics);
        context.SaveChanges();
        string url = baseUri;

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task TopicsController_GetEndpoint_ReturnsTopic()
    {
        // Arrange
        string catKey = Cats[0].Key;
        string name = Topics[0].Name;
        string url = baseUri +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(name)}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<TopicFromDb>();
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(catKey);
        result?.Name.Should().Be(name);
    }
    [Fact]
    public async Task TopicsController_GetEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        string url = baseUri + $"ThisShouldBeNoValidKey/ok";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }

}
