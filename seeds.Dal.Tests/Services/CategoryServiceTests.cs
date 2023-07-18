using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class CategoryServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly CategoryService _service;
    public CategoryServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new(_baseService);
    }

    [Fact]
    public async Task CatService_GetCatByKeyAsync_ReturnsCategory()
    {
        // Arrange
        string key = "ABC";
        Category cat = new() { Key = key, Name = "ABeCe" };
        A.CallTo(() => _baseService.GetDalModelAsync<Category>(A<string>.Ignored))
            .Returns<Category?>(cat);

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
            .Returns<Category?>(null);

        // Act
        var result = await _service.GetCategoryByKeyAsync("N0C");

        // Assert
        result.Should().BeNull();
    }
}
