using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System.Net;

namespace seeds.Dal.Tests.Services;

public class CategoryServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly ICategoryService _service;
    public CategoryServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = A.Fake<ICategoryService>();
    }

    [Fact]
    public async Task CatService_GetCatByKeyAsync_ReturnsCategory()
    {
        // Arrange
        string key = "ABC";
        var cat = new Category() { Key = key, Name = "ABeCe" };
        A.CallTo(() => _baseService.GetDalModelAsync<Category>(A<string>.Ignored))
            .Returns(cat);

        // Act
        var result = await _service.GetCategoryByKeyAsync(key);

        // Assert
        result.Should().NotBeNull();
        result?.Key.Should().Be(key);
    }
    [Fact]
    public async Task CatService_GetCatByKeyAsync_IfNotExistReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<Category>(A<string>.Ignored))
            .Returns(null);

        // Act
        var result = await _service.GetCategoryByKeyAsync("N0C");

        // Assert
        result.Should().BeNull();
    }
}
