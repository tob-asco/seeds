using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;

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

    #region Unit Testing
    [Fact]
    public async Task UsersController_GetUserByUsernameAsync_ReturnsUser()
    {
        //Arrange
        string username = "tobi5";

        //Act
        var result = await _controller.GetUserByUsernameAsync(username);

        //Assert
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        var user = Assert.IsAssignableFrom<User>(actionResult.Value);
        user.Username.Should().Be(username);
    }

    [Fact]
    public async Task UsersController_GetUserByUsernameAsync_IfNotExistReturnsNotFound()
    {
        //Arrange
        string username = "franz";

        //Act
        var result = await _controller.GetUserByUsernameAsync(username);

        //Assert
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UsersController_PostUserAsync_ReturnsSuccess()
    {
        //Arrange
        User User = new User()
        {
            Username = "tobi",
            Password = "",
            Email = ""
        };

        //Act
        var result = await _controller.PostUserAsync(User);

        //Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var postedUser = Assert.IsType<User>(actionResult.Value);
        postedUser.Should().BeEquivalentTo(User);
    }

    /* post the same user twice (id auto-generated) and
     * assert returned Conflict due to (various) uniqueness constraints
     */
    [Fact]
    public async Task UsersController_PostUserAsync_ReturnsConflictDuplicate()
    {
        //Arrange
        User User = new User()
        {
            Username = "tobi",
            Password = "",
            Email = ""
        };

        //Act
        var actionResultOk = await _controller.PostUserAsync(User);
        var actionResultBad = await _controller.PostUserAsync(User);

        //Assert
        //actionResultOk.Result.Should().BeOfType<CreatedAtActionResult>();
        actionResultBad.Result.Should().BeOfType<ConflictObjectResult>();
    }

    #endregion
}
