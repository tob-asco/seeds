using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class UserIdeaInteractionControllerTests : ApiControllerTestsBase
{
    private readonly UserIdeaInteractionsController _controller;
    public List<User> Users { get; } = new();
    public List<Idea> Ideas { get; } = new();
    public List<UserIdeaInteraction> Uiis { get; } = new();
    private readonly int existingUiiUsernameAndIdeaId = 3;
    private readonly int noUiiUsernameAndIdeaId = 4;

    public UserIdeaInteractionControllerTests()
        :base(baseUri: "api/UserIdeaInteractions/")
    {
        _controller = new(context);
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 22; i++)
        {
            Users.Add(new()
            {
                Username = $"tobi #{i}?_", //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
            Ideas.Add(new()
            {
                Title = "Idea #" + i
            });
        }
        if (!context.User.Any()) { context.User.AddRange(Users); }
        if (!context.Idea.Any()) { context.Idea.AddRange(Ideas); }
        Uiis.Add(new UserIdeaInteraction()
        {
            Username = Users[existingUiiUsernameAndIdeaId].Username,
            IdeaId = Ideas[existingUiiUsernameAndIdeaId].Id,
            Upvoted = true,
            Downvoted = true,
        });
        if (!context.UserIdeaInteraction.Any())
        {
            context.UserIdeaInteraction.AddRange(Uiis);
        }
        var uii = context.UserIdeaInteraction.Find(
            Users[noUiiUsernameAndIdeaId].Username,
            Ideas[noUiiUsernameAndIdeaId].Id);
        if (uii != null) { context.UserIdeaInteraction.Remove(uii); }
    }

    [Fact]
    public async Task UiiController_GetIdeaInteractionsOfUserEndpoint_ReturnsItselfs()
    {
        //Arrange
        string username = Users[existingUiiUsernameAndIdeaId].Username;
        string url = baseUri + $"{HttpUtility.UrlEncode(username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<UserIdeaInteraction>>();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(context.UserIdeaInteraction.Where(
            uii => uii.Username == username));
    }
    [Fact]
    public async Task UiiController_GetIdeaInteractionsOfUserEndpoint_IfUserNotExistsReturnsEmpty()
    {
        //Arrange
        string url = baseUri + $"notAuser";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<UserIdeaInteraction>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }
    [Fact]
    public async Task UiiController_GetEndpoint_ReturnsItself()
    {
        //Arrange
        string username = Users[existingUiiUsernameAndIdeaId].Username;
        int ideaId = Ideas[existingUiiUsernameAndIdeaId].Id;
        string url = baseUri + $"{HttpUtility.UrlEncode(username)}/{ideaId}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<UserIdeaInteraction>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
        result?.IdeaId.Should().Be(ideaId);
    }
    [Fact]
    public async Task UiiController_GetEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        string username = Users[noUiiUsernameAndIdeaId].Username;
        int ideaId = Ideas[noUiiUsernameAndIdeaId].Id;
        string url = baseUri + $"{HttpUtility.UrlEncode(username)}/{ideaId}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task UiiController_PutEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        string username = Users[existingUiiUsernameAndIdeaId].Username;
        int ideaId = Ideas[existingUiiUsernameAndIdeaId].Id;
        UserIdeaInteraction uii = new()
        {
            Username = username,
            IdeaId = ideaId,
            Upvoted = false,
            Downvoted = true,
        };
        string url = baseUri + $"{HttpUtility.UrlEncode(username)}/{ideaId}";

        //Act
        var response = await _httpClient.PutAsync(url,JsonContent.Create(uii));

        //Assert
        response.Should().BeSuccessful();
        context.UserIdeaInteraction.Should().ContainEquivalentOf(uii);
    }
    [Fact]
    public async Task UiiController_PutEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        string username = Users[noUiiUsernameAndIdeaId].Username;
        int ideaId = Ideas[noUiiUsernameAndIdeaId].Id;
        UserIdeaInteraction uii = new()
        {
            Username = username,
            IdeaId = ideaId,
        };
        string url = baseUri + $"{HttpUtility.UrlEncode(username)}/{ideaId}";

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(uii));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task UiiController_PostEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        UserIdeaInteraction uii = new()
        {
            Username = Users[noUiiUsernameAndIdeaId].Username,
            IdeaId = Ideas[noUiiUsernameAndIdeaId].Id,
            Upvoted = true,
        };
        string url = "api/UserIdeaInteractions";

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(uii));

        //Assert
        response.Should().BeSuccessful();
        context.UserIdeaInteraction.Should().ContainEquivalentOf(uii);
    }
    [Fact]
    public async Task UiiController_PostEndpoint_IfExistReturnsConflict()
    {
        //Arrange
        UserIdeaInteraction uii = new()
        {
            Username = Users[existingUiiUsernameAndIdeaId].Username,
            IdeaId = Ideas[existingUiiUsernameAndIdeaId].Id,
            Upvoted = true,
        };
        string url = "api/UserIdeaInteractions";

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(uii));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }
    [Fact]
    public async Task UiiController_CountUpvotesEndpoint_ReturnsCorrectUpvoteCount()
    {
        //Arrange
        int ideaId = Ideas[existingUiiUsernameAndIdeaId].Id;
        string url = baseUri + $"{ideaId}/upvotes";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<int>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().Be(1);
    }
    [Fact]
    public async Task UiiController_CountUpvotesEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int ideaId = -1;
        string url = baseUri + $"{ideaId}/upvotes";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task UiiController_CountDownvotesEndpoint_ReturnsCorrectDownvotesCount()
    {
        //Arrange
        int ideaId = Ideas[existingUiiUsernameAndIdeaId].Id;
        string url = baseUri + $"{ideaId}/downvotes";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<int>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().Be(1);
    }
    [Fact]
    public async Task UiiController_CountDownvotesEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int ideaId = -1;
        string url = baseUri + $"{ideaId}/downvotes";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
