using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
    public async Task UsersService_GetUsersAsync_ReturnsUsers()
    {
        #region Arrange
        string uname1 = "tobi1"; string uname2 = "tobi2";
        var users = new List<User>()
        {
            new User { Username = uname1 },
            new User { Username = uname2 },
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<User>>(A<string>.Ignored))
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
        A.CallTo(() => _baseService.GetDalModelAsync<List<User>>(
            A<string>.Ignored))
            .Returns<List<User>?>(null);

        // Act
        var result = await _service.GetUsersAsync();

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public async Task UsersService_GetUserByUsernameAsync_ReturnsUser()
    {
        // Arrange
        string uname = "tobi1";
        User user = new() { Username = uname, };
        A.CallTo(() => _baseService.GetDalModelAsync<User>(A<string>.Ignored))
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
        A.CallTo(() => _baseService.GetDalModelAsync<User>(
            A<string>.Ignored))
            .Returns<User?>(null);

        // Act
        var result = await _service.GetUserByUsernameAsync("");

        // Assert
        result.Should().BeNull();
    }
}
