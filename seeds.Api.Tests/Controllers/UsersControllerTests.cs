using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Api.Tests.Controllers;

public class UsersControllerTests
{
    private readonly User originalTobi;

    /* This "function factory" is used for every test
     * to fake the DB, from YT (teddy smith):
     */
    private async Task<seedsApiContext> GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new seedsApiContext(options);
        databaseContext.Database.EnsureCreated();
        if (!(await databaseContext.User.AnyAsync()))
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
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }
    public UsersControllerTests()
    {
        originalTobi = new User()
        {
            //Id = -1,
            Username = "tobi",
            Password = "",
            Email = ""
        };
    }

    [Fact]
    public async void UsersController_GetUsers_ReturnsLongList()
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new UsersController(dbContext);

        //Act
        var result = await controller.GetUsers();

        //Assert
        result.Should().NotBeNull();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<User>>>(result);
        var userList = Assert.IsAssignableFrom<IEnumerable<User>>(actionResult.Value);
        userList.Should().HaveCountGreaterThan(5); //10, in fact
    }

    [Fact]
    public async void UsersController_GetUserByUsername_ReturnsUser()
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new UsersController(dbContext);
        string username = "tobi5";

        //Act
        var result = await controller.GetUserByUsernameAsync(username);

        //Assert
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        var user = Assert.IsAssignableFrom<User>(actionResult.Value);
        user.Username.Should().Be(username);
    }

    [Fact]
    public async void UsersController_GetUserByUsername_IfNotExistReturnsNotFound()
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new UsersController(dbContext);
        string username = "franz";

        //Act
        var result = await controller.GetUserByUsernameAsync(username);

        //Assert
        var actionResult = Assert.IsType<ActionResult<User>>(result);
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async void UsersController_PostUser_ReturnsSuccess()
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new UsersController(dbContext);

        //Act
        var result = await controller.PostUserAsync(originalTobi);

        //Assert
        var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var postedUser = Assert.IsType<User>(actionResult.Value);
        postedUser.Should().BeEquivalentTo(originalTobi);
    }

    /* post the same user twice (id auto-generated) and
     * assert returned Conflict due to (various) uniqueness constraints
     */
    [Fact]
    public async void UsersController_PostUser_ReturnsConflictDuplicate()
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new UsersController(dbContext);

        //Act
        var actionResultOk = await controller.PostUserAsync(originalTobi);
        var actionResultBad = await controller.PostUserAsync(originalTobi);

        //Assert
        //actionResultOk.Result.Should().BeOfType<CreatedAtActionResult>();
        actionResultBad.Result.Should().BeOfType<ConflictObjectResult>();
    }
}
