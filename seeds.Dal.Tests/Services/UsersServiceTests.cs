using FakeItEasy;
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
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly UsersService _service;
    public UsersServiceTests()
    {
        _httpClientWrapper = A.Fake<IHttpClientWrapper>();
        _service = new UsersService(_httpClientWrapper);
    }

    [Fact]
    public async void UsersService_GetUsers_ReturnsUsers()
    {
        #region Arrange
        var users = new List<User>()
        {
            new User { Username = "tobi1" },
            new User { Username = "tobi2" },
        };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(users)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetUsers();

        // Assert
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async void UsersService_GetUserByUsername_ReturnsUser()
    {
        #region Arrange
        var user = new User { Username = "tobi" };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(user)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetUserByUsername(user.Username);

        // Assert
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async void UsersService_GetUserByUsername_IfNotFoundReturnsNull()
    {
        #region Arrange
        var user = new User { Username = "tobi" };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetUserByUsername(user.Username);

        // Assert
        result.Should().BeNull();
    }
}
