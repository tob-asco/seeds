using seeds.Dal.Dto.FromDb;
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
    private readonly ICatagUserPreferenceService cupService;
    private readonly ITagService tagService;
    private readonly CatagPreferencesService service;

    public CatagPreferenceServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<ICatagUserPreferenceService>();
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
        CatagUserPreference cup1 = new() { CategoryKey = key1, Value = val1 };
        CatagUserPreference cup2 = new() { CategoryKey = key2, Value = val2 };
        A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
            key1, A<string>.Ignored, null))
            .Returns(cup1);
        A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
            key2, A<string>.Ignored, null))
            .Returns(cup2);
        string tagName = "tag";
        List<TagFromDb> tags = new()
        {
            new(){ CategoryKey=key1, Name=tagName }
        };
        CatagUserPreference cupTag = new()
        {
            CategoryKey = key1,
            TagName = tagName,
            Value = val1
        };
        A.CallTo(() => tagService.GetTagsAsync())
            .Returns(tags);
        A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
            tags[0].CategoryKey, A<string>.Ignored, tags[0].Name))
            .Returns(cupTag);
        #endregion

        // Act
        var result = await service.GetCatagPreferencesAsync();

        // Assert
        result.Should().HaveCount(3);
        result[0]?.Preference.Should().Be(val1);
        result[1]?.Preference.Should().Be(val2);
        result[2]?.TagName.Should().NotBeNull();
        result[2]?.Preference.Should().Be(val1);
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
    [Fact]
    public async Task CatagPrefService_GetCatagPreferencesAsync_IfNoTagThrows()
    {
        // Arrange
        A.CallTo(() => tagService.GetTagsAsync())
            .Returns<List<TagFromDb>>(new());

        // Act
        Func<Task> act = async () => await service.GetCatagPreferencesAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
