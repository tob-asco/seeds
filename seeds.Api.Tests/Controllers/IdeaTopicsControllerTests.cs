using AutoMapper;
using seeds.Api.Controllers;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class IdeaTopicsControllerTests : ApiControllerTestsBase
{
    private readonly IdeaTopicsController _controller;
    private readonly IMapper mapper;
    public List<Idea> Ideas { get; } = new();
    public List<Topic> Topics { get; } = new();
    public List<IdeaTopic> IdeaTopics { get; } = new();
    private readonly int indexForIdeaWith3IdeaTopics = 0;
    private readonly int indexForIdeasAndTopicsWithIdeaTopic = 3;
    private readonly int indexForIdeasAndTopicsWithoutIdeaTopic = 4;

    public IdeaTopicsControllerTests()
        :base(baseUri: "api/IdeaTopics/")
    {
        mapper = A.Fake<IMapper>();
        _controller = new(context, mapper);
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 22; i++)
        {
            Ideas.Add(new()
            {
                Title = "Idea #" + i
            });
            Topics.Add(new()
            {
                CategoryKey = "NoC",
                Name = "topic #" + i
            });
        }
        if (!context.Idea.Any()) { context.Idea.AddRange(Ideas); }
        if (!context.Topic.Any()) { context.Topic.AddRange(Topics); }
        for (int i = 0; i <= 2; i++)
        {
            IdeaTopics.Add(new()
            {
                IdeaId = Ideas[indexForIdeaWith3IdeaTopics].Id,
                TopicId = Topics[i].Id
            });
        }
        IdeaTopics.Add(new()
        {
            IdeaId = Ideas[indexForIdeasAndTopicsWithIdeaTopic].Id,
            TopicId = Topics[indexForIdeasAndTopicsWithIdeaTopic].Id
        });
        if (!context.IdeaTopic.Any()) { context.IdeaTopic.AddRange(IdeaTopics); }
        var ideaTopic = context.IdeaTopic.Find(
            Ideas[indexForIdeasAndTopicsWithoutIdeaTopic].Id,
            Topics[indexForIdeasAndTopicsWithoutIdeaTopic].Id);
        if (ideaTopic != null) { context.IdeaTopic.Remove(ideaTopic); }
    }

    [Fact]
    public async Task IdeaTopicsController_GetTopicsOfIdeaEndpoint_Returns3Items()
    {
        //Arrange
        int ideaId = Ideas[indexForIdeaWith3IdeaTopics].Id;
        string url = baseUri + $"{ideaId}";
        //List<Topic> mapperParameter = new();
        //A.CallTo(() => mapper.Map<List<TopicFromDb>>(A<List<Topic>?>._))
        //    .Invokes((List<Topic>? source) => mapperParameter = source!)
        //    .Returns(new() { new(),new(),new() }); // 3 TopicsFromDb

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<TopicFromDb>>();
        result.Should().NotBeNull();
        result?.Should().HaveCount(3);
        //mapperParameter.Should().HaveCount(3);
    }
    [Fact]
    public async Task IdeaTopicsController_GetTopicsOfIdeaEndpoint_IfNoTopicReturnsEmptyList()
    {
        // Arrange
        int ideaId = Ideas[indexForIdeasAndTopicsWithoutIdeaTopic].Id;
        string url = baseUri + $"{ideaId}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<IdeaTopic>>();
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
    [Fact]
    public async Task IdeaTopicsController_PostEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        IdeaTopic ideaTopic = new()
        {
            IdeaId = Ideas[indexForIdeasAndTopicsWithoutIdeaTopic].Id,
            TopicId = Topics[indexForIdeasAndTopicsWithoutIdeaTopic].Id
        };
        string url = baseUri;

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(ideaTopic));

        //Assert
        response.Should().BeSuccessful();
        context.IdeaTopic.Should().ContainEquivalentOf(ideaTopic);
    }
    [Fact]
    public async Task IdeaTopicsController_PostEndpoint_IfExistReturnsConflict()
    {
        //Arrange
        IdeaTopic ideaTopic = new()
        {
            IdeaId = Ideas[indexForIdeasAndTopicsWithIdeaTopic].Id,
            TopicId = Topics[indexForIdeasAndTopicsWithIdeaTopic].Id
        };
        string url = baseUri;

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(ideaTopic));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }
    [Fact]
    public async Task IdeaTopicsController_DeleteEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        int ideaId = Ideas[indexForIdeasAndTopicsWithIdeaTopic].Id;
        string catKey = Topics[indexForIdeasAndTopicsWithIdeaTopic].CategoryKey;
        string topicName = Topics[indexForIdeasAndTopicsWithIdeaTopic].Name;
        string url = baseUri +
            $"{ideaId}/{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(topicName)}";

        //Act
        var response = await _httpClient.DeleteAsync(url);

        //Assert
        response.Should().BeSuccessful();
        context.IdeaTopic.Should().NotContainEquivalentOf(
            IdeaTopics.FirstOrDefault(it =>
                it.IdeaId == Ideas[indexForIdeasAndTopicsWithIdeaTopic].Id &&
                it.TopicId == Topics[indexForIdeasAndTopicsWithIdeaTopic].Id
            )!);
    }
    [Fact]
    public async Task IdeaTopicsController_DeleteEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        int ideaId = Ideas[indexForIdeasAndTopicsWithoutIdeaTopic].Id;
        string catKey = Topics[indexForIdeasAndTopicsWithoutIdeaTopic].CategoryKey;
        string topicName = Topics[indexForIdeasAndTopicsWithoutIdeaTopic].Name;
        string url = baseUri +
            $"{ideaId}/{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(topicName)}";

        // Act
        var response = await _httpClient.DeleteAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
