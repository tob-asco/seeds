using FakeItEasy.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;

namespace seeds.Api.Tests.Controllers;

public class CategoriesControllerTests : ApiBaseControllerTests
{
    private readonly CategoriesController _controller;
    public List<Category> Categories { get; set; } = new();

    public CategoriesControllerTests()
    {
        _controller = new(_context);
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Categories.Add(
            new Category()
            {
                Key = $"Cat{i}",
                Name = $"Category{i}"
            });
        }
        if(!_context.Category.Any()) { _context.Category.AddRange(Categories); }
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
