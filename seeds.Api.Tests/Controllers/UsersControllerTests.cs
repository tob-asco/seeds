using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Tests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _controller;
    private readonly User _originalTobi;

    /* This "function factory" is used for every test
     * to fake the DB, from YT (teddy smith):
     */
    private static seedsApiContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new seedsApiContext(options);
        databaseContext.Database.EnsureCreated();
        if (!(databaseContext.User.Any()))
        {
            //we save 10 instances, but why?
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.User.Add(
                new User()
                {
                    //Id = "Pikachu",
                    Username = "tobi"+i, //unique
                    Password = "tobi",
                    Email = "tobi"+i+"@tobi.com", //unique
                });
                databaseContext.SaveChanges();
            }
        }
        return databaseContext;
    }
    public UsersControllerTests()
    {
        _controller = new UsersController(GetDatabaseContext());
        _originalTobi = new User()
        {
            //Id = -1,
            Username = "tobi",
            Password = "",
            Email = ""
        };
    }

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

        //Act
        var result = await _controller.PostUserAsync(_originalTobi);

        //Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var postedUser = Assert.IsType<User>(actionResult.Value);
        postedUser.Should().BeEquivalentTo(_originalTobi);
    }

    /* post the same user twice (id auto-generated) and
     * assert returned Conflict due to (various) uniqueness constraints
     */
    [Fact]
    public async Task UsersController_PostUserAsync_ReturnsConflictDuplicate()
    {
        //Arrange

        //Act
        var actionResultOk = await _controller.PostUserAsync(_originalTobi);
        var actionResultBad = await _controller.PostUserAsync(_originalTobi);

        //Assert
        //actionResultOk.Result.Should().BeOfType<CreatedAtActionResult>();
        actionResultBad.Result.Should().BeOfType<ConflictObjectResult>();
    }
}
