using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.Tests.Services;

public class FeedEntriesServiceTests
{
    private readonly IGlobalService globalService;
    private readonly IIdeasService ideasService;
    private readonly ICategoryService categoryService;
    private readonly ICatagUserPreferenceService cupService;
    private readonly IUserIdeaInteractionService uiiService;
    private readonly ICatagPreferencesService catagPrefService;
    private readonly FeedEntriesService service;
    public FeedEntriesServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        ideasService = A.Fake<IIdeasService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<ICatagUserPreferenceService>();
        uiiService = A.Fake<IUserIdeaInteractionService>();
        catagPrefService = A.Fake<ICatagPreferencesService>();
        service = new(globalService, ideasService, uiiService, catagPrefService);
    }

    [Fact]
    public async Task FeedEntriesService_GetFeedEntriesPaginatedAsync_ReturnsItselfs()
    {
        #region Arrange
        int pageIndex = 1; int pageSize = 2;
        CategoryDto cat1 = new() { Key = "Cat1" };
        CategoryDto cat2 = new() { Key = "Cat2" };
        List<IdeaFromDb> ideaPage = new() {
            new(){ CategoryKey = cat1.Key},
            new(){ CategoryKey = cat2.Key},
        };
        List<CatagPreference> catagPrefs = new()
        {
            new CatagPreference { CategoryKey = cat1.Key },
            new CatagPreference { CategoryKey = cat2.Key },
        };
        A.CallTo(() => ideasService.GetIdeasPaginatedAsync(
            A<int>.Ignored, A<int>.Ignored, A<string>.Ignored, A<bool>.Ignored))
            .Returns(ideaPage);
        A.CallTo(() => catagPrefService.GetTagPreferencesOfIdeaAsync(
            A<IdeaFromDb>.Ignored))
            .Returns(catagPrefs);
        A.CallTo(() => uiiService.CountVotesAsync(A<int>.Ignored))
            .Returns(0);
        #endregion

        // Act
        var result = await service.GetFeedEntriesPaginatedAsync(pageIndex, pageSize);

        // Assert
        result.Should().HaveCount(ideaPage.Count);
        result[0]?.Idea.CategoryKey.Should().Be(ideaPage[0].CategoryKey);
        result[0]?.CatagPreferences.Should().BeEquivalentTo(catagPrefs);
        result[1]?.Idea.CategoryKey.Should().Be(ideaPage[1].CategoryKey);
    }
    [Fact]
    public async Task FeedEntriesService_GetFeedEntriesPaginatedAsync_IfNoIdeasReturnsEmptyList()
    {
        #region Arrange
        int page = 1; int pageSize = 2;
        A.CallTo(() => ideasService.GetIdeasPaginatedAsync(page, 5, "CreationTime", true))
            .Returns<List<IdeaFromDb>>(new());
        #endregion

        // Act
        var result = await service.GetFeedEntriesPaginatedAsync(page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
}
