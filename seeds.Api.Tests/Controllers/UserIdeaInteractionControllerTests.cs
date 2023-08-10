using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

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
    {
        _controller = new(_context);
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 22; i++)
        {
            Users.Add(new()
            {
                Username = $"tobi{i}", //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
            Ideas.Add(new()
            {
                Title = "Idea #" + i
            });
        }
        if (!_context.User.Any()) { _context.User.AddRange(Users); }
        if (!_context.Idea.Any()) { _context.Idea.AddRange(Ideas); }
        Uiis.Add(new UserIdeaInteraction()
        {
            Username = Users[existingUiiUsernameAndIdeaId].Username,
            IdeaId = Ideas[existingUiiUsernameAndIdeaId].Id,
            Upvoted = true,
            Downvoted = true,
        });
        if (!_context.UserIdeaInteraction.Any())
        {
            _context.UserIdeaInteraction.AddRange(Uiis);
        }
        var uii = _context.UserIdeaInteraction.Find(
            Users[noUiiUsernameAndIdeaId].Username,
            Ideas[noUiiUsernameAndIdeaId].Id);
        if (uii != null) { _context.UserIdeaInteraction.Remove(uii); }
    }

    [Fact]
    public async Task UiiController_GetEndpoint_ReturnsItself()
    {
        //Arrange
        string username = Users[existingUiiUsernameAndIdeaId].Username;
        int ideaId = Ideas[existingUiiUsernameAndIdeaId].Id;
        string url = $"/api/UserIdeaInteractions/{username}/{ideaId}";

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
        string url = $"/api/UserIdeaInteractions/{username}/{ideaId}";

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
        string url = $"/api/UserIdeaInteractions/{username}/{ideaId}";

        //Act
        var response = await _httpClient.PutAsync(url,JsonContent.Create(uii));

        //Assert
        response.Should().BeSuccessful();
        _context.UserIdeaInteraction.Should().ContainEquivalentOf(uii);
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
        string url = $"/api/UserIdeaInteractions/{username}/{ideaId}";

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
        _context.UserIdeaInteraction.Should().ContainEquivalentOf(uii);
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
        string url = $"/api/UserIdeaInteractions/{ideaId}/upvotes";

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
        string url = $"/api/UserIdeaInteractions/{ideaId}/upvotes";

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
        string url = $"/api/UserIdeaInteractions/{ideaId}/downvotes";

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
        string url = $"/api/UserIdeaInteractions/{ideaId}/downvotes";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
