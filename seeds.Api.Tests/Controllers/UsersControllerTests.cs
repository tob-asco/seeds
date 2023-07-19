using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class UsersControllerTests : ApiBaseControllerTests
{
    private readonly UsersController _controller;
    public List<User> Users { get; set; } = new();

    public UsersControllerTests()
    {
        _controller = new(_context);
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
    }

    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Users.Add(
            new User()
            {
                Username = "tobi" + i, //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
        }
        if (!_context.User.Any()) { _context.User.AddRange(Users); }
    }
    [Fact]
    public async Task UsersController_GetUserEndpoint_ReturnsUser()
    {
        //Arrange
        string username = "tobi5";
        string url = $"/api/Users/{username}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<User>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
    }
    [Fact]
    public async Task UsersController_GetUserEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        string username = "franz";
        string url = $"/api/Users/{username}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task UsersController_PostUserEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        User user = new()
        {
            Username = "tooobi",
            Password = "",
            Email = ""
        };
        string url = "/api/Users";
        HttpContent content = JsonContent.Create(user);

        //Act
        var postResponse = await _httpClient.PostAsync(url, content);

        //Assert
        postResponse.Should().BeSuccessful();
        _context.User.Should().ContainEquivalentOf(user);
    }
    /* post the same user twice (id auto-generated) and
     * assert returned Conflict due to (various) uniqueness constraints
     */
    [Fact]
    public async Task UsersController_PostUserEndpoint_ReturnsConflictDuplicate()
    {
        //Arrange
        User user = new User()
        {
            Username = "tobi",
            Password = "",
            Email = ""
        };
        string url = "/api/Users";
        HttpContent content = JsonContent.Create(user);

        //Act
        await _httpClient.PostAsync(url, content);
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }
}
