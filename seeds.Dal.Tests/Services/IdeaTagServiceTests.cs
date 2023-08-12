using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class IdeaTagServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly IdeaTagService _service;
    public IdeaTagServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new IdeaTagService(_baseService);
    }
    [Fact]
    public async Task IdeaTagService_GetTagsOfIdeaAsync_ReturnsItselfs()
    {
        // Arrange
        int ideaId = 20;
        List<IdeaTag> ideaTags = new() { new()
        {
            CategoryKey = "cat0", IdeaId = ideaId, TagName = "tag0"
        }, new()
        {
            CategoryKey = "cat1", IdeaId = ideaId, TagName = "tag1"
        }};
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaTag>>(A<string>.Ignored))
            .Returns(ideaTags);

        // Act
        var result = await _service.GetTagsOfIdeaAsync(ideaId);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(ideaTags.Count);
        result?[0]?.IdeaId.Should().Be(ideaId);
        result?[1]?.IdeaId.Should().Be(ideaId);
    }
    [Fact]
    public async Task IdeaTagService_GetIdeaTagAsync_IfEmptyListResturnsEmptyList()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaTag>>(
            A<string>.Ignored))
            .Returns<List<IdeaTag>?>(new());

        // Act
        var result = await _service.GetTagsOfIdeaAsync(1);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
    [Fact]
    public async Task IdeaTagService_GetIdeaTagAsync_IfNullThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaTag>>(
            A<string>.Ignored))
            .Returns<List<IdeaTag>?>(null);
        A.CallTo(() => _baseService.ThrowGetNullException(A<string>.Ignored))
            .Returns(new());

        // Act
        Func<Task> act = async () => await _service.GetTagsOfIdeaAsync(1); //"1" doesn't matter

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTagService_PostIdeaTagAsync_NoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelBoolReturnAsync(
            A<string>.Ignored, A<IdeaTag>.Ignored))
            .Returns<bool>(true);

        // Act
        Func<Task> act = async () => await _service.PostIdeaTagAsync(new());

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTagService_PostIdeaTagAsync_IfFalseThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelBoolReturnAsync(
            A<string>.Ignored, A<IdeaTag>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.ThrowPostConflictException(A<string>.Ignored))
            .Returns(new());

        // Act
        Func<Task> act = async () => await _service.PostIdeaTagAsync(new());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTagService_DeleteIdeaTagAsync_NoException()
    {
        // Arrange
        A.CallTo(() => _baseService.DeleteAsync(A<string>.Ignored))
            .Returns<bool>(true);

        // Act
        Func<Task> act = async () => await _service.DeleteIdeaTagAsync(0,"","");

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTagService_DeleteIdeaTagAsync_IfFalseThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.DeleteAsync(A<string>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.ThrowDeleteNotFoundException(A<string>.Ignored))
            .Returns(new());

        // Act
        Func<Task> act = async () => await _service.DeleteIdeaTagAsync(0,"","");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
