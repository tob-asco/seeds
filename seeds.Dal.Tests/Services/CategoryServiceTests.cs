using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
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
    public async Task CatService_GetCatByKeyAsync_ReturnsItself()
    {
        // Arrange
        string key = "ABC";
        CategoryFromDb cat = new() { Key = key, Name = "ABeCe" };
        A.CallTo(() => _baseService.GetDalModelAsync<CategoryFromDb>(A<string>.Ignored))
            .Returns<CategoryFromDb?>(cat);

        // Act
        var result = await _service.GetCategoryByKeyAsync(key);

        // Assert
        result.Should().NotBeNull();
        result?.Key.Should().Be(key);
    }
    [Fact]
    public async Task CatService_GetCatByKeyAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<CategoryFromDb>(A<string>.Ignored))
            .Returns<CategoryFromDb?>(null);

        // Act
        Func<Task> act = async () => await _service.GetCategoryByKeyAsync("N0C");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
