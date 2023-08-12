using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.Tests.Services;

public class CatagPreferenceServiceTests
{
    private readonly IGlobalService globalService;
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly ITagService tagService;
    private readonly CatagPreferencesService service;

    public CatagPreferenceServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<ICategoryUserPreferenceService>();
        tagService = A.Fake<ITagService>();
        service = new(globalService, categoryService, cupService, tagService);
    }

    [Fact]
    public async Task CatagPrefService_GetCatagPreferencesAsync_ReturnsItselfs()
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
            key1, A<string>.Ignored, A<string?>.Ignored))
            .Returns(cup1);
        A.CallTo(() => cupService.GetCategoryUserPreferenceAsync(
            key2, A<string>.Ignored, A<string?>.Ignored))
            .Returns(cup2);
        #endregion

        // Act
        var result = await service.GetCatagPreferencesAsync();

        // Assert
        result.Should().HaveCount(2);
        result[0]?.Preference.Should().Be(val1);
        result[1]?.Preference.Should().Be(val2);
    }
    [Fact]
    public async Task CatagPrefService_GetCatagPreferencesAsync_IfNoCatsThrows()
    {
        // Arrange
        A.CallTo(() => categoryService.GetCategoriesAsync())
            .Returns<List<CategoryDto>>(new());

        // Act
        Func<Task> act = async () => await service.GetCatagPreferencesAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
