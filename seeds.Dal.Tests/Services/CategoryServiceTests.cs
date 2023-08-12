using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class CategoryServiceTests
{
    private readonly IDalBaseService baseService;
    private readonly CategoryService service;
    public CategoryServiceTests()
    {
        baseService = A.Fake<IDalBaseService>();
        service = new(baseService);
    }

    [Fact]
    public async Task CatService_GetCategoriesAsync_ReturnsItselfs()
    {
        // Arrange
        string key = "ABC", name = "ABeCe";
        List<CategoryDto> cats = new()
        {
           new() { Key = key, Name = "abceee" },
            new() { Key = "BLA", Name = name },
        };
        A.CallTo(() => baseService
            .GetDalModelAsync<List<CategoryDto>>(A<string>.Ignored))
            .Returns(cats);

        // Act
        var result = await service.GetCategoriesAsync();

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(2);
        result?[0]?.Key.Should().Be(key);
        result?[1]?.Name.Should().Be(name);
    }
    [Fact]
    public async Task CatService_GetCategoriesAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<List<CategoryDto>>(A<string>.Ignored))
            .Returns<List<CategoryDto>?>(null);

        // Act
        Func<Task> act = async () => await service.GetCategoriesAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task CatService_GetCatByKeyAsync_ReturnsItself()
    {
        // Arrange
        string key = "ABC";
        CategoryDto cat = new() { Key = key, Name = "ABeCe" };
        A.CallTo(() => baseService.GetDalModelAsync<CategoryDto>(A<string>.Ignored))
            .Returns<CategoryDto?>(cat);

        // Act
        var result = await service.GetCategoryByKeyAsync(key);

        // Assert
        result.Should().NotBeNull();
        result?.Key.Should().Be(key);
    }
    [Fact]
    public async Task CatService_GetCatByKeyAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<CategoryDto>(A<string>.Ignored))
            .Returns<CategoryDto?>(null);

        // Act
        Func<Task> act = async () => await service.GetCategoryByKeyAsync("N0C");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
