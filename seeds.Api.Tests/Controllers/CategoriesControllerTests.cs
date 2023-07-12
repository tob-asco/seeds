using FakeItEasy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Tests.Controllers;

public class CategoriesControllerTests
{
    private readonly CategoriesController _controller;
    public CategoriesControllerTests()
    {
        _controller = new CategoriesController(GetDatabaseContext());
    }
    private static seedsApiContext GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new seedsApiContext(options);
        databaseContext.Database.EnsureCreated();
        if (!(databaseContext.Category.Any()))
        {
            for (int i = 1; i <= 10; i++)
            {
                databaseContext.Category.Add(
                new Category()
                {
                    Key = $"Cat{i}",
                    Name = $"Category{i}"
                });
                databaseContext.SaveChanges();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async Task CategoriesController_GetCategoriesAsync_ReturnsAtLeastSameNumberOfCategories()
    {
        //Arrange

        //Act
        var result = await _controller.GetCategoriesAsync();

        //Assert
        result.Should().NotBeNull();
        var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
        var resultList = Assert.IsAssignableFrom<IEnumerable<Category>>(actionResult.Value);
        resultList.Should().HaveCountGreaterThan(9);
    }

    [Fact]
    public async Task CategoriesController_GetCategoryAsync_ReturnsCategory()
    {
        //Arrange
        string key = "Cat2";

        //Act
        var result = await _controller.GetCategoryAsync(key);

        //Assert
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        var cat = Assert.IsAssignableFrom<Category>(actionResult.Value);
        cat.Key.Should().Be(key);
    }

    [Fact]
    public async Task CategoriesController_GetCategoryAsync_IfNotExistReturnsNotFound()
    {
        //Arrange
        string key = "BLö";

        //Act
        var result = await _controller.GetCategoryAsync(key);

        //Assert
        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }
}
