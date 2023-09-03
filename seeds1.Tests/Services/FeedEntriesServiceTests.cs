using seeds.Dal.Dto.ForMaui;
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
    private readonly FeedEntriesService service;
    public FeedEntriesServiceTests()
    {
        globalService = A.Fake<IGlobalService>();
        ideasService = A.Fake<IIdeasService>();
        categoryService = A.Fake<ICategoryService>();
        service = new(globalService, ideasService);
    }

    [Fact]
    public async Task FeedEntriesService_GetUserFeedentriesPaginatedAsync_ReturnsItselfs()
    {
        #region Arrange
        int pageIndex = 1; int pageSize = 2;
        int ideaId = 9;
        List<Feedentry> ufes = new() {
            new(){ Idea = new() { Id = ideaId } },
            new(){ Idea = new() { Id = ideaId } },
        };
        A.CallTo(() => ideasService.GetFeedentriesPaginatedAsync(
            A<int>.Ignored, A<int>.Ignored, A<string>.Ignored, A<bool>.Ignored))
            .Returns(ufes);
        A.CallTo(() => globalService.GetIdeaInteractions())
            .Returns(new Dictionary<int, UserIdeaInteraction>() {{ ideaId, new() }});
        #endregion

        // Act
        var result = await service.GetUserFeedentriesPaginatedAsync(pageIndex, pageSize);

        // Assert
        result.Should().HaveCount(ufes.Count);
        result[0]?.Idea.Id.Should().Be(ideaId);
        result[1]?.Idea.Id.Should().Be(ideaId);
    }
    [Fact]
    public async Task FeedEntriesService_GetFeedEntriesPaginatedAsync_IfNoFesReturnsEmptyList()
    {
        #region Arrange
        int page = 1; int pageSize = 2;
        A.CallTo(() => ideasService.GetFeedentriesPaginatedAsync(page, 5, "CreationTime", true))
            .Returns<List<Feedentry>>(new());
        #endregion

        // Act
        var result = await service.GetUserFeedentriesPaginatedAsync(page, pageSize);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
}
