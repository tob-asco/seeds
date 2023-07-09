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

public class IdeasControllerTests
{
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
            for (int i = 1; i <= 22; i++)
            {
                databaseContext.Idea.Add(
                new Idea()
                {
                    Title = "Idea #" + i
                });
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }
    public IdeasControllerTests()
    {
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(3, 7)]
    public async void IdeasController_GetIdeasPaginated_ReturnsListOfGivenLength(int page, int maxPageSize)
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new IdeasController(dbContext);

        //Act
        var result = await controller.GetIdeasPaginatedAsync(page, maxPageSize);

        //Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Idea>>>(result);
        var ideaList = Assert.IsAssignableFrom<IEnumerable<Idea>>(actionResult.Value);
        ideaList.Should().HaveCount(maxPageSize);
    }

    [Fact]
    public async void IdeasController_GetIdeasPaginated_IfNotEnoughIdeasReturnsNotFound()
    {
        //Arrange
        var dbContext = await GetDatabaseContext();
        var controller = new IdeasController(dbContext);
        int page = 4; int maxPageSize = 10;

        //Act
        var actionResult = await controller.GetIdeasPaginatedAsync(page, maxPageSize);

        //Assert
        actionResult.Result.Should().NotBeOfType<BadRequestResult>();
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }
}
