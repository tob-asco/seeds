using seeds.Api.Pages;
using seeds.Dal.Dto.ForMaui;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;
using System.Drawing.Printing;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class IdeasControllerTests : ApiControllerTestsBase
{
    public List<Idea> Ideas { get; set; } = new();
    public Tag Tag { get; set; } = new();
    public User User { get; set; } = new();
    readonly int ideasIndexWithUpvote = 0;

    public IdeasControllerTests()
        : base(baseUri: "api/Ideas/")
    {
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh context
        context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        if (!context.Tag.Any()) { context.Tag.Add(Tag); }
        if (!context.User.Any()) { context.User.Add(User); }

        Random random = new();
        for (int i = 0; i <= 22; i++)
        {
            Ideas.Add(new Idea()
            {
                CreatorName = User.Username,
                Title = "Idea #" + i,
                CreationTime = new(2023, 07, random.Next(1, 31)),
                Tags = new List<Tag> {Tag}
            });
        }
        if (!context.Idea.Any()) { context.Idea.AddRange(Ideas); }

        if (!context.UserIdeaInteraction.Any())
        {
            context.UserIdeaInteraction.Add(new()
            {
                IdeaId = Ideas[ideasIndexWithUpvote].Id,
                Username = User.Username,
                Upvoted = true,
            });
        }
    }

    [Theory]
    [InlineData(1, 10)] // pageSize length
    [InlineData(4, 7)] // At least one
    [InlineData(6, 12)] // None
    public async Task IdeasController_GetPaginatedEndpoint_ReturnsSuccessAndCorrectSizeList(
        int pageIndex, int pageSize)
    {
        //Arrange
        string url = baseUri + $"page/{pageIndex}?pageSize={pageSize}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<IdeaFromDb>>();
        if (Ideas.Count >= (pageIndex * pageSize))
        {
            result.Should().HaveCount(pageSize);
        }
        else if (Ideas.Count < (pageIndex * pageSize) &&
            Ideas.Count > ((pageIndex - 1) * pageSize))
        {
            result.Should().HaveCount(Ideas.Count - ((pageIndex - 1) * pageSize));
        }
        else
        {
            result.Should().HaveCount(0);
        }
    }
    [Fact]
    public async Task IdeasController_GetPaginatedEndpoint_OrdersByCreationTime()
    {
        //Arrange
        context.Idea.Add(new() { CreationTime = DateTime.MinValue });
        context.Idea.Add(new() { CreationTime = DateTime.MaxValue });
        context.SaveChanges();
        string url = baseUri + $"page/1?pageSize={Ideas.Count + 10}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<IdeaFromDb>>();
        result.Should().NotBeNull();
        var ordered = result?.OrderByDescending(x => x.CreationTime).ToList();
        ordered?.Should().BeEquivalentTo(result);
    }
    [Fact]
    public async Task IdeasController_GetFeedentryPageEndpoint_OrdersByCreationTime()
    {
        //Arrange
        context.Idea.Add(new() { CreationTime = DateTime.MinValue });
        context.Idea.Add(new() { CreationTime = DateTime.MaxValue });
        context.SaveChanges();
        string url = baseUri + $"feedentryPage/1?pageSize={Ideas.Count + 10}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<IdeaFromDb>>();
        result.Should().NotBeNull();
        var ordered = result?.OrderByDescending(x => x.CreationTime).ToList();
        ordered?.Should().BeEquivalentTo(result);
    }
    [Fact]
    public async Task IdeasController_GetFeedentryPageEndpoint_ReturnsCorrectCount()
    {
        //Arrange
        int pageSize = Ideas.Count;
        string url = baseUri + $"feedentryPage/1?" +
            $"pageSize={pageSize}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<Feedentry>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(Ideas.Count);
    }
    [Fact]
    public async Task IdeasController_GetFeedentryPageEndpoint_ReturnsTagsAndUpvotes()
    {
        //Arrange
        string url = baseUri + $"feedentryPage/1?" +
            $"pageSize={Ideas.Count}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<Feedentry>>();
        result.Should().NotBeNull();
        result.Should().Contain(fe => fe.Tags.Count > 0 &&
            fe.Idea.Id == Ideas[ideasIndexWithUpvote].Id);
        result.Should().Contain(fe => fe.Upvotes > 0);
    }
    [Fact]
    public async Task IdeasController_GetIdeaEndpoint_ReturnsIdeaDto()
    {
        //Arrange
        int id = Ideas[0].Id;
        string url = baseUri + $"{id}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<IdeaFromDb>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Id.Should().Be(id);

    }
    [Fact]
    public async Task IdeasController_GetIdeaEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int id = -900;
        string url = baseUri + $"{id}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task IdeasController_PutIdeaEndpoint_ReturnsSuccess()
    {
        //Arrange
        int id = Ideas[0].Id;
        IdeaFromDb idea = new()
        {
            Id = id,
            Title = "new title"
        };
        string url = baseUri + $"{id}";
        var content = JsonContent.Create(idea);

        //Act
        var response = await _httpClient.PutAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
    }
    [Fact]
    public async Task IdeasController_PutIdeaEndpoint_UpdatesTitleAndLeavesIdAndCreationTime()
    {
        //Arrange
        int index = 0;
        int id = Ideas[index].Id;
        DateTime time = DateTime.MinValue;
        IdeaFromDb idea = new()
        {
            Id = id,
            CreationTime = time,
            Title = Guid.NewGuid().ToString(),
        };
        string url = baseUri + $"{id}";
        var content = JsonContent.Create(idea);

        //Act
        await _httpClient.PutAsync(url, content);

        //Assert
        context.Idea.Find(id).Should().NotBeNull();
        context.Idea.Find(id)?.CreationTime.Should().Be(time);
        context.Idea.Find(id)?.Title.Should().NotBe(Ideas[index].Title);
    }
    [Fact]
    public async Task IdeasController_PutIdeaEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int id = -900;
        string url = baseUri + $"{id}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task IdeasController_PostIdeaEndpoint_ReturnsSuccess()
    {
        //Arrange
        IdeaToDb idea = new()
        {
            Title = "new title",
        };
        string url = baseUri;
        var content = JsonContent.Create(idea);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
    }
    [Fact]
    public async Task IdeasController_PostIdeaEndpoint_ReturnsIdeaWithPkFromDb()
    {
        //Arrange
        string title = Guid.NewGuid().ToString();
        IdeaToDb idea = new()
        {
            Title = title,
        };
        string url = baseUri;
        var content = JsonContent.Create(idea);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        var result = await response.Content.ReadFromJsonAsync<Idea>();
        result.Should().NotBeNull();
        result?.Id.Should().Be(
            context.Idea.FirstOrDefault(i => i.Title == title)!.Id);
    }
}
