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
    public async Task UiiService_GetUiiAsync_ReturnsUii()
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
        var result = await _service.GetUserIdeaInteractionAsync("N0C",0);

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
}
