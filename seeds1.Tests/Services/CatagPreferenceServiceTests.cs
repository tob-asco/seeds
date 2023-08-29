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
    private readonly IUserPreferenceService cupService;
    private readonly ITagService tagService;
    private readonly IIdeaTagService ideaTagService;
    private readonly CatagPreferencesService service;

    public CatagPreferenceServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<IUserPreferenceService>();
        tagService = A.Fake<ITagService>();
        ideaTagService = A.Fake<IIdeaTagService>();
        service = new(
            globalService, categoryService, cupService, tagService, ideaTagService);
    }

    //[Fact]
    //public async Task CatagPrefService_GetTagPreferencesOfIdeaAsync_ReturnsItselfs()
    //{
    //    #region Arrange
    //    string key0 = "Cat1";
    //    string key1 = "Cat2";
    //    int val0 = 1;
    //    int val1 = -1;
    //    string tagName = "tag";
    //    List<TagFromDb> tags = new()
    //    {
    //        new(){ CategoryKey=key0, Name=tagName },
    //        new(){ CategoryKey=key1, Name=tagName },
    //    };
    //    UserPreference tup0 = new()
    //    {
    //        CategoryKey = key0,
    //        TagName = tagName,
    //        Value = val0
    //    };
    //    UserPreference tup1 = new()
    //    {
    //        CategoryKey = key1,
    //        TagName = tagName,
    //        Value = val1
    //    };
    //    A.CallTo(() => ideaTagService.GetTagsOfIdeaAsync(A<int>.Ignored))
    //        .Returns<List<TagFromDb>>(tags);
    //    A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
    //        tags[0].CategoryKey, A<string>.Ignored, tags[0].Name))
    //        .Returns(tup0);
    //    A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
    //        tags[1].CategoryKey, A<string>.Ignored, tags[1].Name))
    //        .Returns(tup1);
    //    #endregion

    //    // Act
    //    var result = await service.GetTagPreferencesOfIdeaAsync(new());

    //    // Assert
    //    result.Should().HaveCount(2);
    //    result[0]?.Preference.Should().Be(val0);
    //    result[1]?.Preference.Should().Be(val1);
    //}
    //[Fact]
    //public async Task CatagPrefService_GetTagPreferencesOfIdeaAsync_IfNoTagsReturnsEmpty()
    //{
    //    // Arrange
    //    A.CallTo(() => ideaTagService.GetTagsOfIdeaAsync(A<int>.Ignored))
    //        .Returns<List<TagFromDb>>(new());

    //    // Act
    //    var result = await service.GetTagPreferencesOfIdeaAsync(new());

    //    // Assert
    //    result.Should().NotBeNull();
    //    result?.Should().HaveCount(0);
    //}
    //[Fact]
    //public async Task CatagPrefService_GetCatagPreferencesAsync_ReturnsItselfs()
    //{
    //    #region Arrange
    //    string key1 = "Cat1";
    //    string key2 = "Cat2";
    //    List<CategoryDto> cats = new()
    //    {
    //        new() {Key=key1},
    //        new() {Key=key2},
    //    };
    //    A.CallTo(() => categoryService.GetCategoriesAsync())
    //        .Returns(cats);
    //    int val1 = 1;
    //    int val2 = -1;
    //    UserPreference cup1 = new() { CategoryKey = key1, Value = val1 };
    //    UserPreference cup2 = new() { CategoryKey = key2, Value = val2 };
    //    A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
    //        key1, A<string>.Ignored, null))
    //        .Returns(cup1);
    //    A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
    //        key2, A<string>.Ignored, null))
    //        .Returns(cup2);
    //    string tagName = "tag";
    //    List<TagFromDb> tags = new()
    //    {
    //        new(){ CategoryKey=key1, Name=tagName }
    //    };
    //    UserPreference tup = new()
    //    {
    //        CategoryKey = key1,
    //        TagName = tagName,
    //        Value = val1
    //    };
    //    A.CallTo(() => tagService.GetTagsAsync())
    //        .Returns<List<TagFromDb>>(tags);
    //    A.CallTo(() => cupService.GetCatagUserPreferenceAsync(
    //        tags[0].CategoryKey, A<string>.Ignored, tags[0].Name))
    //        .Returns(tup);
    //    #endregion

    //    // Act
    //    var result = await service.GetCatagPreferencesAsync();

    //    // Assert
    //    result.Should().HaveCount(3);
    //    result[0]?.Preference.Should().Be(val1);
    //    result[1]?.Preference.Should().Be(val2);
    //    result[2]?.TagName.Should().NotBeNull();
    //    result[2]?.Preference.Should().Be(val1);
    //}
    //[Fact]
    //public async Task CatagPrefService_GetCatagPreferencesAsync_IfNoCatsThrows()
    //{
    //    // Arrange
    //    A.CallTo(() => categoryService.GetCategoriesAsync())
    //        .Returns<List<CategoryDto>>(new());

    //    // Act
    //    Func<Task> act = async () => await service.GetCatagPreferencesAsync();

    //    // Assert
    //    await act.Should().ThrowAsync<Exception>();
    //}
    //[Fact]
    //public async Task CatagPrefService_GetCatagPreferencesAsync_IfNoTagThrows()
    //{
    //    // Arrange
    //    A.CallTo(() => tagService.GetTagsAsync())
    //        .Returns<List<TagFromDb>>(new());

    //    // Act
    //    Func<Task> act = async () => await service.GetCatagPreferencesAsync();

    //    // Assert
    //    await act.Should().ThrowAsync<Exception>();
    //}
}
