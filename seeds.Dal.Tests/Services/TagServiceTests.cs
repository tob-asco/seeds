using seeds.Dal.Dto.ToAndFromDb;
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
        List<TagDto> tags = new()
        {
           new() { CategoryKey = key, Name = "abceee" },
            new() { CategoryKey = "BLA", Name = name },
        };
        A.CallTo(() => baseService.GetDalModelAsync<List<TagDto>>(A<string>.Ignored))
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
        A.CallTo(() => baseService.GetDalModelAsync<List<TagDto>>(A<string>.Ignored))
            .Returns<List<TagDto>?>(null);

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
        TagDto tag = new() { CategoryKey = key, Name = name };
        A.CallTo(() => baseService.GetDalModelAsync<TagDto>(A<string>.Ignored))
            .Returns<TagDto?>(tag);

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
        A.CallTo(() => baseService.GetDalModelAsync<TagDto>(A<string>.Ignored))
            .Returns<TagDto?>(null);

        // Act
        Func<Task> act = async () => await service.GetTagAsync("", "");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
