using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class IdeaTopicServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly IdeaTopicService _service;
    public IdeaTopicServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new IdeaTopicService(_baseService);
    }
    [Fact]
    public async Task IdeaTopicService_GetTopicsOfIdeaAsync_ReturnsItselfs()
    {
        // Arrange
        string topicName0 = "foo", topicName1 = "bar";
        List<TopicFromDb> topics = new() {
            new() { Name = topicName0, },
            new() { Name = topicName1, }
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<TopicFromDb>>(A<string>.Ignored))
                .Returns(topics);

        // Act
        var result = await _service.GetTopicsOfIdeaAsync(0);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(topics.Count);
        result?[0]?.Name.Should().Be(topicName0);
        result?[1]?.Name.Should().Be(topicName1);
    }
    [Fact]
    public async Task IdeaTopicService_GetTopicsOfIdeaAsync_IfEmptyListResturnsEmptyList()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<TopicFromDb>>(
            A<string>.Ignored))
            .Returns<List<TopicFromDb>?>(new());

        // Act
        var result = await _service.GetTopicsOfIdeaAsync(1);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
    [Fact]
    public async Task IdeaTopicService_GetTopicsOfIdeaAsync_IfNullThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<TopicFromDb>>(
            A<string>.Ignored))
            .Returns<List<TopicFromDb>?>(null);
        A.CallTo(() => _baseService.ThrowGetNullException(A<string>.Ignored))
            .Returns(new Exception());

        // Act
        Func<Task> act = async () => await _service.GetTopicsOfIdeaAsync(1); //"1" doesn't matter

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTopicService_PostIdeaTopicAsync_NoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelBoolReturnAsync(
            A<string>.Ignored, A<IdeaTopic>.Ignored))
            .Returns<bool>(true);

        // Act
        Func<Task> act = async () => await _service.PostIdeaTopicAsync(new());

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTopicService_PostIdeaTopicAsync_IfFalseThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelBoolReturnAsync(
            A<string>.Ignored, A<IdeaTopic>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.ThrowPostConflictException(A<string>.Ignored))
            .Returns(new Exception());

        // Act
        Func<Task> act = async () => await _service.PostIdeaTopicAsync(new());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTopicService_DeleteIdeaTopicAsync_NoException()
    {
        // Arrange
        A.CallTo(() => _baseService.DeleteAsync(A<string>.Ignored))
            .Returns<bool>(true);

        // Act
        Func<Task> act = async () => await _service.DeleteIdeaTopicAsync(0, "", "");

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeaTopicService_DeleteIdeaTopicAsync_IfFalseThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.DeleteAsync(A<string>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.ThrowDeleteNotFoundException(A<string>.Ignored))
            .Returns(new Exception());

        // Act
        Func<Task> act = async () => await _service.DeleteIdeaTopicAsync(0, "", "");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
