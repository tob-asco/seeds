using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class TopicServiceTests
{
    private readonly IDalBaseService baseService;
    private readonly TopicService service;
    public TopicServiceTests()
    {
        baseService = A.Fake<IDalBaseService>();
        service = new(baseService);
    }

    [Fact]
    public async Task TopicService_GetTopicsAsync_ReturnsItselfs()
    {
        // Arrange
        string key = "ABC", name = "ABeCe";
        List<TopicFromDb> topics = new()
        {
           new() { CategoryKey = key, Name = "abceee" },
            new() { CategoryKey = "BLA", Name = name },
        };
        A.CallTo(() => baseService.GetDalModelAsync<List<TopicFromDb>>(A<string>.Ignored))
            .Returns(topics);

        // Act
        var result = await service.GetTopicsAsync();

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(2);
        result?[0]?.CategoryKey.Should().Be(key);
        result?[1]?.Name.Should().Be(name);
    }
    [Fact]
    public async Task TopicService_GetTopicsAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<List<TopicFromDb>>(A<string>.Ignored))
            .Returns<List<TopicFromDb>?>(null);

        // Act
        Func<Task> act = async () => await service.GetTopicsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task TopicService_GetTopicAsync_ReturnsItself()
    {
        // Arrange
        string key = "ABC", name = "ABeCe";
        TopicFromDb topic = new() { CategoryKey = key, Name = name };
        A.CallTo(() => baseService.GetDalModelAsync<TopicFromDb>(A<string>.Ignored))
            .Returns<TopicFromDb?>(topic);

        // Act
        var result = await service.GetTopicAsync(key, name);

        // Assert
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(key);
        result?.Name.Should().Be(name);
    }
    [Fact]
    public async Task TopicService_GetTopicAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<TopicFromDb>(A<string>.Ignored))
            .Returns<TopicFromDb?>(null);

        // Act
        Func<Task> act = async () => await service.GetTopicAsync("", "");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
