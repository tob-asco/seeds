using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Tests.Controllers;

public class IdeasControllerTests : ApiBaseControllerTests
{
    private readonly IdeasController _controller;
    public List<Idea> Ideas { get; set; } = new();

    public IdeasControllerTests()
    {
        _controller = new(_context);

        DummyUpTheProperties();

        if(!_context.Idea.Any()) { _context.Idea.AddRange(Ideas); }

        _context.SaveChanges();
    }
    private void DummyUpTheProperties()
    {
        for (int i = 1; i <= 22; i++)
        {
            Ideas.Add(
            new Idea()
            {
                Title = "Idea #" + i
            });
        }
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(3, 7)]
    public async void IdeasController_GetIdeasPaginated_ReturnsListOfGivenLength(int page, int maxPageSize)
    {
        //Arrange

        //Act
        var result = await _controller.GetIdeasPaginatedAsync(page, maxPageSize);

        //Assert
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Idea>>>(result);
        var ideaList = Assert.IsAssignableFrom<IEnumerable<Idea>>(actionResult.Value);
        ideaList.Should().HaveCount(maxPageSize);
    }

    [Fact]
    public async void IdeasController_GetIdeasPaginated_IfNotEnoughIdeasReturnsNotFound()
    {
        //Arrange
        int page = 4;
        int maxPageSize = 10;

        //Act
        var actionResult = await _controller.GetIdeasPaginatedAsync(page, maxPageSize);

        //Assert
        actionResult.Result.Should().NotBeOfType<BadRequestResult>();
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }
}
