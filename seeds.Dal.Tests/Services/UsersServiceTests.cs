using seeds.Dal.Dto.ToAndFromDb;
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
        var users = new List<UserDto>()
        {
            new() { Username = uname1 },
            new() { Username = uname2 },
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<UserDto>>(A<string>.Ignored))
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
        A.CallTo(() => _baseService.GetDalModelAsync<List<UserDto>>(
            A<string>.Ignored))
            .Returns<List<UserDto>?>(null);

        // Act
        Func<Task> act = async () => await _service.GetUsersAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task UsersService_GetUserByUsernameAsync_ReturnsItself()
    {
        // Arrange
        string uname = "tobi1";
        UserDto user = new() { Username = uname, };
        A.CallTo(() => _baseService.GetDalModelAsync<UserDto>(A<string>.Ignored))
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
        A.CallTo(() => _baseService.GetDalModelAsync<UserDto>(
            A<string>.Ignored))
            .Returns<UserDto?>(null);

        // Act
        var result = await _service.GetUserByUsernameAsync("");

        // Assert
        result.Should().BeNull();
    }
}
