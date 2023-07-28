using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.Services;

namespace seeds1.Tests.Services;

public class FeedEntriesServiceTests
{
    private readonly IGlobalService globalService;
    private readonly IIdeasService ideasService;
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly IUserIdeaInteractionService uiiService;
    private readonly FeedEntriesService service;
    public FeedEntriesServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        ideasService = A.Fake<IIdeasService>();
        categoryService = A.Fake<ICategoryService>();
        cupService = A.Fake<ICategoryUserPreferenceService>();
        uiiService = A.Fake<IUserIdeaInteractionService>();
        service = new(globalService, ideasService, categoryService, cupService, uiiService);
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
        A.CallTo(() => ideasService.GetIdeasPaginatedAsync(
            A<int>.Ignored, A<int>.Ignored, A<string>.Ignored, A<bool>.Ignored))
            .Returns(ideaPage);
        A.CallTo(() => uiiService.CountVotesAsync(A<int>.Ignored))
            .Returns(0);
        A.CallTo(() => categoryService.GetCategoryByKeyAsync(cat1.Key))
            .Returns(cat1);
        A.CallTo(() => categoryService.GetCategoryByKeyAsync(cat2.Key))
            .Returns(cat2);
        A.CallTo(() => cupService.GetCategoryUserPreferenceAsync(
            A<string>.Ignored, A<string>.Ignored))
            .Returns<CategoryUserPreference>(new());
        #endregion

        // Act
        var result = await service.GetFeedEntriesPaginatedAsync(pageIndex, pageSize);

        // Assert
        result.Should().HaveCount(ideaPage.Count);
        result[0]?.Idea.CategoryKey.Should().Be(ideaPage[0].CategoryKey);
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
