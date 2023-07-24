using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class UserIdeaInteractionServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly UserIdeaInteractionService _service;
    public UserIdeaInteractionServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new(_baseService);
    }
    [Fact]
    public async Task UiiService_GetUiiAsync_ReturnsItself()
    {
        #region Arrange
        string uname = "dude";
        int id = 1;
        UserIdeaInteraction uii = new()
        {
            Username = uname,
            IdeaId = id,
        };
        A.CallTo(() => _baseService.GetDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored))
            .Returns(uii);
        #endregion

        // Act
        var result = await _service.GetUserIdeaInteractionAsync(uname, id);

        // Assert
        result.Should().NotBeNull();
        result?.Username.Should().Be(uname);
        result?.IdeaId.Should().Be(id);
    }
    [Fact]
    public async Task UiiService_GetUiiAsync_IfNotExistReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored))
            .Returns<UserIdeaInteraction?>(null);

        // Act
        var result = await _service.GetUserIdeaInteractionAsync("N0C", 0);

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public async Task UiiService_PutUiiAsync_ReturnsTrue()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(true);
        string uname = "dude";
        int id = 2;
        bool up = true;
        bool down = true;

        // Act
        var result = await _service.PutUserIdeaInteractionAsync(uname, id, up, down);

        // Assert
        result.Should().Be(true);
    }
    [Fact]
    public async Task UiiService_PutUiiAsync_IfNotSuccessReturnsFalse()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(false);
        string uname = "dude";
        int id = 2;
        bool up = true;
        bool down = true;

        // Act
        var result = await _service.PutUserIdeaInteractionAsync(uname, id, up, down);

        // Assert
        result.Should().Be(false);
    }
    [Fact]
    public async Task UiiService_PostUiiAsync_ReturnsTrue()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(true);

        // Act
        var result = await _service.PostUserIdeaInteractionAsync(new());

        // Assert
        result.Should().Be(true);
    }
    [Fact]
    public async Task UiiService_PostUiiAsync_IfNotSuccessReturnsFalse()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(false);

        // Act
        var result = await _service.PostUserIdeaInteractionAsync(new());

        // Assert
        result.Should().Be(false);
    }
    [Fact]
    public async Task UiiService_PostOrPutUiiAsync_IfExistNoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.PutDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(true);

        // Act
        Func<Task> act = async () => await _service.PostOrPutUserIdeaInteractionAsync(new());

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task UiiService_PostOrPutUiiAsync_IfNotFoundNoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(true);
        A.CallTo(() => _baseService.PutDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.PostOrPutUserIdeaInteractionAsync(new());

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task UiiService_PostOrPutUiiAsync_IfErrorThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.PutDalModelAsync<UserIdeaInteraction>(
            A<string>.Ignored, A<UserIdeaInteraction>.Ignored))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.PostOrPutUserIdeaInteractionAsync(new());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task UiiService_CountVotesAsync_ReturnsCorrectVotes()
    {
        // Arrange
        int ideaId = 1;
        int upvotes = 7;
        int downvotes = 2;
        string upvotesUrl = $"api/UserIdeaInteractions/{ideaId}/upvotes";
        string downvotesUrl = $"api/UserIdeaInteractions/{ideaId}/downvotes";
        A.CallTo(() => _baseService.GetNonDalModelAsync<int>(upvotesUrl))
            .Returns(upvotes);
        A.CallTo(() => _baseService.GetNonDalModelAsync<int>(downvotesUrl))
            .Returns(downvotes);

        // Act
        var result = await _service.CountVotesAsync(ideaId);

        // Assert
        result.Should().Be(upvotes - downvotes);
    }
    [Fact]
    public async Task UiiService_CountVotesAsync_IfUpvoteGetErrorThrows()
    {
        // Arrange
        int ideaId = 1;
        int downvotes = 2;
        string upvotesUrl = $"api/UserIdeaInteractions/{ideaId}/upvotes";
        string downvotesUrl = $"api/UserIdeaInteractions/{ideaId}/downvotes";
        A.CallTo(() => _baseService.GetNonDalModelAsync<int>(upvotesUrl))
            .Throws<Exception>();
        A.CallTo(() => _baseService.GetNonDalModelAsync<int>(downvotesUrl))
            .Returns(downvotes);

        // Act
        Func<Task> act = async () => await _service.CountVotesAsync(ideaId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task UiiService_CountVotesAsync_IfDownvoteGetErrorThrows()
    {
        // Arrange
        int ideaId = 1;
        int upvotes = 7;
        string upvotesUrl = $"api/UserIdeaInteractions/{ideaId}/upvotes";
        string downvotesUrl = $"api/UserIdeaInteractions/{ideaId}/downvotes";
        A.CallTo(() => _baseService.GetNonDalModelAsync<int>(upvotesUrl))
            .Returns(upvotes);
        A.CallTo(() => _baseService.GetNonDalModelAsync<int>(downvotesUrl))
            .Throws<Exception>();

        // Act
        Func<Task> act = async () => await _service.CountVotesAsync(ideaId);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
