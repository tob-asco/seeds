using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.Tests.Services;

public class CatPrefServiceTests
{
    private readonly IGlobalService globalService;
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly CategoryPreferencesService service;

    public CatPrefServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<ICategoryUserPreferenceService>();
        service = new(globalService, categoryService, cupService);
    }

    [Fact]
    public async Task CatPrefService_GetCatPreferencesAsync_ReturnsItselfs()
    {
        #region Arrange
        string key1 = "Cat1";
        string key2 = "Cat2";
        List<CategoryDto> cats = new()
        {
            new() {Key=key1},
            new() {Key=key2},
        };
        A.CallTo(() => categoryService.GetCategoriesAsync())
            .Returns(cats);
        int val1 = 1;
        int val2 = -1;
        CategoryUserPreference cup1 = new() { CategoryKey = key1, Value = val1 };
        CategoryUserPreference cup2 = new() { CategoryKey = key2, Value = val2 };
        A.CallTo(() => cupService.GetCategoryUserPreferenceAsync(
            key1, A<string>.Ignored))
            .Returns(cup1);
        A.CallTo(() => cupService.GetCategoryUserPreferenceAsync(
            key2, A<string>.Ignored))
            .Returns(cup2);
        #endregion

        // Act
        var result = await service.GetCatPreferencesAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0]?.Value.Should().Be(val1);
        result[1]?.Value.Should().Be(val2);
    }
    [Fact]
    public async Task CatPrefService_GetCatPreferencesAsync_IfNoCatsThrows()
    {
        #region Arrange
        A.CallTo(() => categoryService.GetCategoriesAsync())
            .Returns<List<CategoryDto>?>(null);
        #endregion

        // Act
        Func<Task> act = async () => await service.GetCatPreferencesAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task CatPrefService_GetCatPreferencesAsync_IfCupMissingThrows()
    {
        #region Arrange
        string key1 = "Cat1";
        string key2 = "Cat2";
        List<CategoryDto> cats = new()
        {
            new() {Key=key1},
            new() {Key=key2},
        };
        A.CallTo(() => categoryService.GetCategoriesAsync())
            .Returns(cats);
        int val1 = 1;
        int val2 = -1;
        CategoryUserPreference cup1 = new() { CategoryKey = key1, Value = val1 };
        CategoryUserPreference cup2 = new() { CategoryKey = key2, Value = val2 };
        A.CallTo(() => cupService.GetCategoryUserPreferenceAsync(
            key1, A<string>.Ignored))
            .Returns(cup1);
        A.CallTo(() => cupService.GetCategoryUserPreferenceAsync(
            key2, A<string>.Ignored))
            .Returns<CategoryUserPreference?>(null);
        #endregion

        // Act
        Func<Task> act = async () => await service.GetCatPreferencesAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
