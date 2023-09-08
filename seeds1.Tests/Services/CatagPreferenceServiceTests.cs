using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.Tests.Services;

public class CatopicPreferenceServiceTests
{
    private readonly IGlobalService globalService;
    private readonly IStaticService staticService;
    private readonly ICategoryService categoryService;
    private readonly IUserPreferenceService cupService;
    private readonly ITopicService topicService;
    private readonly IIdeaTopicService ideaTopicService;
    private readonly CatopicPreferencesService service;

    public CatopicPreferenceServiceTests()
    {
        staticService = A.Fake<IStaticService>();
        globalService = A.Fake<IGlobalService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<IUserPreferenceService>();
        topicService = A.Fake<ITopicService>();
        ideaTopicService = A.Fake<IIdeaTopicService>();
        service = new(
            staticService, globalService, categoryService, cupService, topicService, ideaTopicService);
    }

    //[Fact]
    //public async Task CatopicPrefService_GetTopicPreferencesOfIdeaAsync_ReturnsItselfs()
    //{
    //    #region Arrange
    //    string key0 = "Cat1";
    //    string key1 = "Cat2";
    //    int val0 = 1;
    //    int val1 = -1;
    //    string topicName = "topic";
    //    List<TopicFromDb> topics = new()
    //    {
    //        new(){ CategoryKey=key0, Name=topicName },
    //        new(){ CategoryKey=key1, Name=topicName },
    //    };
    //    UserPreference tup0 = new()
    //    {
    //        CategoryKey = key0,
    //        TopicName = topicName,
    //        Value = val0
    //    };
    //    UserPreference tup1 = new()
    //    {
    //        CategoryKey = key1,
    //        TopicName = topicName,
    //        Value = val1
    //    };
    //    A.CallTo(() => ideaTopicService.GetTopicsOfIdeaAsync(A<int>.Ignored))
    //        .Returns<List<TopicFromDb>>(topics);
    //    A.CallTo(() => cupService.GetCatopicUserPreferenceAsync(
    //        topics[0].CategoryKey, A<string>.Ignored, topics[0].Name))
    //        .Returns(tup0);
    //    A.CallTo(() => cupService.GetCatopicUserPreferenceAsync(
    //        topics[1].CategoryKey, A<string>.Ignored, topics[1].Name))
    //        .Returns(tup1);
    //    #endregion

    //    // Act
    //    var result = await service.GetTopicPreferencesOfIdeaAsync(new());

    //    // Assert
    //    result.Should().HaveCount(2);
    //    result[0]?.Preference.Should().Be(val0);
    //    result[1]?.Preference.Should().Be(val1);
    //}
    //[Fact]
    //public async Task CatopicPrefService_GetTopicPreferencesOfIdeaAsync_IfNoTopicsReturnsEmpty()
    //{
    //    // Arrange
    //    A.CallTo(() => ideaTopicService.GetTopicsOfIdeaAsync(A<int>.Ignored))
    //        .Returns<List<TopicFromDb>>(new());

    //    // Act
    //    var result = await service.GetTopicPreferencesOfIdeaAsync(new());

    //    // Assert
    //    result.Should().NotBeNull();
    //    result?.Should().HaveCount(0);
    //}
    //[Fact]
    //public async Task CatopicPrefService_GetCatopicPreferencesAsync_ReturnsItselfs()
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
    //    A.CallTo(() => cupService.GetCatopicUserPreferenceAsync(
    //        key1, A<string>.Ignored, null))
    //        .Returns(cup1);
    //    A.CallTo(() => cupService.GetCatopicUserPreferenceAsync(
    //        key2, A<string>.Ignored, null))
    //        .Returns(cup2);
    //    string topicName = "topic";
    //    List<TopicFromDb> topics = new()
    //    {
    //        new(){ CategoryKey=key1, Name=topicName }
    //    };
    //    UserPreference tup = new()
    //    {
    //        CategoryKey = key1,
    //        TopicName = topicName,
    //        Value = val1
    //    };
    //    A.CallTo(() => topicService.GetTopicsAsync())
    //        .Returns<List<TopicFromDb>>(topics);
    //    A.CallTo(() => cupService.GetCatopicUserPreferenceAsync(
    //        topics[0].CategoryKey, A<string>.Ignored, topics[0].Name))
    //        .Returns(tup);
    //    #endregion

    //    // Act
    //    var result = await service.GetCatopicPreferencesAsync();

    //    // Assert
    //    result.Should().HaveCount(3);
    //    result[0]?.Preference.Should().Be(val1);
    //    result[1]?.Preference.Should().Be(val2);
    //    result[2]?.TopicName.Should().NotBeNull();
    //    result[2]?.Preference.Should().Be(val1);
    //}
    //[Fact]
    //public async Task CatopicPrefService_GetCatopicPreferencesAsync_IfNoCatsThrows()
    //{
    //    // Arrange
    //    A.CallTo(() => categoryService.GetCategoriesAsync())
    //        .Returns<List<CategoryDto>>(new());

    //    // Act
    //    Func<Task> act = async () => await service.GetCatopicPreferencesAsync();

    //    // Assert
    //    await act.Should().ThrowAsync<Exception>();
    //}
    //[Fact]
    //public async Task CatopicPrefService_GetCatopicPreferencesAsync_IfNoTopicThrows()
    //{
    //    // Arrange
    //    A.CallTo(() => topicService.GetTopicsAsync())
    //        .Returns<List<TopicFromDb>>(new());

    //    // Act
    //    Func<Task> act = async () => await service.GetCatopicPreferencesAsync();

    //    // Assert
    //    await act.Should().ThrowAsync<Exception>();
    //}
}
