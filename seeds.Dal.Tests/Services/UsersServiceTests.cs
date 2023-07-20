using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class UsersServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly UsersService _service;
    public UsersServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new UsersService(_baseService);
    }
    [Fact]
    public async Task UsersService_GetUsersAsync_ReturnsItselfs()
    {
        #region Arrange
        string uname1 = "tobi1"; string uname2 = "tobi2";
        var users = new List<UserDtoApi>()
        {
            new() { Username = uname1 },
            new() { Username = uname2 },
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<UserDtoApi>>(A<string>.Ignored))
            .Returns(users);
        #endregion

        // Act
        var result = await _service.GetUsersAsync();

        // Assert
        result.Should().NotBeNull();
        result?[0].Username.Should().Be(uname1);
        result?[1].Username.Should().Be(uname2);
    }
    [Fact]
    public async Task UsersService_GetUsersAsync_IfErrorReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<UserDtoApi>>(
            A<string>.Ignored))
            .Returns<List<UserDtoApi>?>(null);

        // Act
        var result = await _service.GetUsersAsync();

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public async Task UsersService_GetUserByUsernameAsync_ReturnsItself()
    {
        // Arrange
        string uname = "tobi1";
        UserDtoApi user = new() { Username = uname, };
        A.CallTo(() => _baseService.GetDalModelAsync<UserDtoApi>(A<string>.Ignored))
            .Returns(user);

        // Act
        var result = await _service.GetUserByUsernameAsync(uname);

        // Assert
        result.Should().NotBeNull();
        result?.Username.Should().Be(uname);
    }
    [Fact]
    public async Task UsersService_GetUserByUsernameAsync_IfNotFoundReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<UserDtoApi>(
            A<string>.Ignored))
            .Returns<UserDtoApi?>(null);

        // Act
        var result = await _service.GetUserByUsernameAsync("");

        // Assert
        result.Should().BeNull();
    }
}
