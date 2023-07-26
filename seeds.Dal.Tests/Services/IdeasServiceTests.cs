using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;

namespace seeds.Dal.Tests.Services;

public class IdeasServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly IdeasService _service;
    public IdeasServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new IdeasService(_baseService);
    }
    [Fact]
    public async void IdeasService_GetIdeaAsync_ReturnsItself()
    {
        // Arrange
        int id = 1;
        IdeaFromDb idea = new() { Id = id, Title = "title", };
        A.CallTo(() => _baseService.GetDalModelAsync<IdeaFromDb>(A<string>.Ignored))
            .Returns(idea);

        // Act
        var result = await _service.GetIdeaAsync(id);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(id);
    }
    [Fact]
    public async Task IdeasService_GetIdeaAsync_IfNotExistThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<IdeaFromDb>(
            A<string>.Ignored))
            .Returns<IdeaFromDb?>(null);

        // Act
        Func<Task> act = async () => await _service.GetIdeaAsync(1); //"1" doesn't matter

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async void IdeasService_GetIdeasPaginatedAsync_ReturnsAllItselfs()
    {
        #region Arrange
        int page = 2; int maxPageSize = 10;
        int id1 = 1; int id2 = 2;
        List<IdeaFromDb> users = new()
        {
            new IdeaFromDb{ Id = id1, Title = "1st Idea" },
            new IdeaFromDb{ Id = id2, Title = "2nd Idea" },
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaFromDb>>(A<string>.Ignored))
            .Returns(users);
        #endregion

        // Act
        var result = await _service.GetIdeasPaginatedAsync(page, maxPageSize);

        // Assert
        result.Should().NotBeNull();
        result?[0].Id.Should().Be(id1);
        result?[1].Id.Should().Be(id2);
    }
    [Fact]
    public async Task IdeasService_GetIdeasPaginatedAsync_IfNotExistReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaFromDb>>(
            A<string>.Ignored))
            .Returns<List<IdeaFromDb>?>(null);

        // Act
        var result = await _service.GetIdeasPaginatedAsync(1, 10);

        // Assert
        result.Should().BeNull();
    }
}
