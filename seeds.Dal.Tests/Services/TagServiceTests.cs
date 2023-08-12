using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class TagServiceTests
{
    private readonly IDalBaseService baseService;
    private readonly TagService service;
    public TagServiceTests()
    {
        baseService = A.Fake<IDalBaseService>();
        service = new(baseService);
    }

    [Fact]
    public async Task TagService_GetTagsAsync_ReturnsItselfs()
    {
        // Arrange
        string key = "ABC", name = "ABeCe";
        List<TagFromDb> tags = new()
        {
           new() { CategoryKey = key, Name = "abceee" },
            new() { CategoryKey = "BLA", Name = name },
        };
        A.CallTo(() => baseService.GetDalModelAsync<List<TagFromDb>>(A<string>.Ignored))
            .Returns(tags);

        // Act
        var result = await service.GetTagsAsync();

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(2);
        result?[0]?.CategoryKey.Should().Be(key);
        result?[1]?.Name.Should().Be(name);
    }
    [Fact]
    public async Task TagService_GetTagsAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<List<TagFromDb>>(A<string>.Ignored))
            .Returns<List<TagFromDb>?>(null);

        // Act
        Func<Task> act = async () => await service.GetTagsAsync();

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task TagService_GetTagAsync_ReturnsItself()
    {
        // Arrange
        string key = "ABC", name = "ABeCe";
        TagFromDb tag = new() { CategoryKey = key, Name = name };
        A.CallTo(() => baseService.GetDalModelAsync<TagFromDb>(A<string>.Ignored))
            .Returns<TagFromDb?>(tag);

        // Act
        var result = await service.GetTagAsync(key, name);

        // Assert
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(key);
        result?.Name.Should().Be(name);
    }
    [Fact]
    public async Task TagService_GetTagAsync_IfBaseReturnsNullThrows()
    {
        // Arrange
        A.CallTo(() => baseService.GetDalModelAsync<TagFromDb>(A<string>.Ignored))
            .Returns<TagFromDb?>(null);

        // Act
        Func<Task> act = async () => await service.GetTagAsync("", "");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
