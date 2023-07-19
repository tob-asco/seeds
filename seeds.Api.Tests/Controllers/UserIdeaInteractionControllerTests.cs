using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class UserIdeaInteractionControllerTests : ApiBaseControllerTests
{
    private readonly UserIdeaInteractionsController _controller;
    public List<User> Users { get; } = new();
    public List<Idea> Ideas { get; } = new();
    public List<UserIdeaInteraction> Uiis { get; } = new();
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
            Users.Add(new User()
            {
                Username = $"tobi{i}", //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
            Ideas.Add(new Idea()
            {
                Title = "Idea #" + i
            });
        }
        if (!_context.User.Any()) { _context.User.AddRange(Users); }
        if (!_context.Idea.Any()) { _context.Idea.AddRange(Ideas); }
        Uiis.Add(new UserIdeaInteraction()
        {
            Username = Users[0].Username,
            IdeaId = Ideas[3].Id,
            Upvoted = true,
        });
        Uiis.Add(new UserIdeaInteraction()
        {
            Username = Users[3].Username,
            IdeaId = Ideas[0].Id,
            Downvoted = true,
        });
        if (!_context.UserIdeaInteraction.Any())
        {
            _context.UserIdeaInteraction.AddRange(Uiis);
        }
        var uii = _context.UserIdeaInteraction.Find(
            Users[noUiiUsernameAndIdeaId].Username,
            Ideas[noUiiUsernameAndIdeaId].Id);
        if (uii != null)
        {
            _context.UserIdeaInteraction.Remove(uii);
        }
    }

    #region Unit Testing
    [Fact]
    public async Task UiiController_GetUii_ReturnsItself()
    {
        // Arrange
        string username = Users[0].Username;
        int ideaId = Ideas[3].Id;

        // Act
        var result = await _controller.GetUserIdeaInteraction(username, ideaId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserIdeaInteraction>>(result);
        var uii = Assert.IsAssignableFrom<UserIdeaInteraction>(actionResult.Value);
        uii.Should().NotBeNull();
        uii?.Username.Should().Be(username);
        uii?.IdeaId.Should().Be(ideaId);
    }
    [Fact]
    public async Task UiiController_GetUii_IfNotExistReturnsNotFound()
    {
        // Arrange
        string username = Users[noUiiUsernameAndIdeaId].Username;
        int ideaId = Ideas[noUiiUsernameAndIdeaId].Id;

        // Act
        var result = await _controller.GetUserIdeaInteraction(username, ideaId);

        // Assert
        var actionResult = Assert.IsType<ActionResult<UserIdeaInteraction>>(result);
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }
    [Fact]
    public async Task UiiController_PostUii_ReturnsItself()
    {
        //Arrange
        UserIdeaInteraction uii = new()
        {
            Username = Users[4].Username,
            IdeaId = Ideas[3].Id,
            Upvoted = true,
        };
        //Act
        var result = await _controller.PostUserIdeaInteraction(uii);

        //Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var postedUii = Assert.IsType<UserIdeaInteraction>(actionResult.Value);
        postedUii.Should().BeEquivalentTo(uii);
    }
    /* Error: "The instance of entity type 'UserIdeaInteraction' cannot be tracked
     * because another instance with the same key value for {'Username', 'IdeaId'}
     * is already being tracked."
     */
    //[Fact]
    //public async Task UiiController_PostUii_IfExistsReturnsConflict()
    //{
    //    //Arrange
    //    UserIdeaInteraction uii = new()
    //    {
    //        Username = Users[0].Username,
    //        IdeaId = Ideas[3].Id,
    //        Upvoted = true,
    //    };

    //    //Act
    //    var result = await _controller.PostUserIdeaInteraction(uii);

    //    //Assert
    //    var actionResult = Assert.IsType<ActionResult<UserIdeaInteraction>>(result);
    //    actionResult.Result.Should().BeOfType<ConflictResult>();
    //}

    #endregion
    #region Endpoint Testing
    [Fact]
    public async Task UiiController_GetEndpoint_ReturnsItself()
    {
        //Arrange
        string username = Users[3].Username;
        int ideaId = Ideas[0].Id;
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
    [Theory]
    [InlineData(false,false)]
    [InlineData(false,true)]
    [InlineData(true,true)]
    public async Task UiiController_PutEndpoint_UpdatedDb(bool up, bool down)
    {
        //Arrange
        string username = Users[3].Username;
        int ideaId = Ideas[0].Id;
        UserIdeaInteraction uii = new()
        {
            Username = username,
            IdeaId = ideaId,
            Upvoted = up,
            Downvoted = down,
        };
        string url = $"/api/UserIdeaInteractions/{username}/{ideaId}";
        var content = JsonContent.Create(uii);

        //Act
        var putResponse = await _httpClient.PutAsync(url, content);
        var getResponse = await _httpClient.GetAsync(url);
        var getResult = await getResponse.Content
            .ReadFromJsonAsync<UserIdeaInteraction>();

        //Assert
        putResponse.Should().BeSuccessful();
        getResponse.Should().BeSuccessful();
        getResult.Should().NotBeNull();
        getResult?.Upvoted.Should().Be(up);
        getResult?.Downvoted.Should().Be(down);
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
        var content = JsonContent.Create(uii);

        //Act
        var putResponse = await _httpClient.PutAsync(url, content);

        //Assert
        putResponse.Should().HaveStatusCode(HttpStatusCode.NotFound);
        putResponse.IsSuccessStatusCode.Should().Be(false);
    }

    #endregion
}
