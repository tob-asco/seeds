using FakeItEasy;
using seeds.Dal.Dto.ForMaui;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
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
    public async Task IdeasService_GetIdeaAsync_ReturnsItself()
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
    public async Task IdeasService_GetIdeasPaginatedAsync_ReturnsAllItselfs()
    {
        #region Arrange
        int page = 2;
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
        var result = await _service.GetIdeasPaginatedAsync(page);

        // Assert
        result.Should().NotBeNull();
        result?[0].Id.Should().Be(id1);
        result?[1].Id.Should().Be(id2);
    }
    [Fact]
    public async Task IdeasService_GetIdeasPaginatedAsync_IfNotExistReturnsEmptyList()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaFromDb>>(
            A<string>.Ignored))
            .Returns<List<IdeaFromDb>?>(new());

        // Act
        var result = await _service.GetIdeasPaginatedAsync(1, 10);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
    [Fact]
    public async Task IdeasService_GetIdeasPaginatedAsync_IfBaseNullThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaFromDb>>(
            A<string>.Ignored))
            .Returns<List<IdeaFromDb>?>(null);

        // Act
        Func<Task> act = async () => await _service.GetIdeasPaginatedAsync(1);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeasService_GetFeedentriesPaginatedAsync_ReturnsAllItselfs()
    {
        #region Arrange
        int page = 2;
        int id1 = 1; int id2 = 2;
        List<Feedentry> returned = new()
        {
            new Feedentry{ Idea = new() { Id = id1, Title = "1st Idea" } },
            new Feedentry{ Idea = new() { Id = id2, Title = "2nd Idea" } },
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<Feedentry>>(A<string>.Ignored))
            .Returns(returned);
        #endregion

        // Act
        var result = await _service.GetFeedentriesPaginatedAsync(page);

        // Assert
        result.Should().NotBeNull();
        result?[0].Idea.Id.Should().Be(id1);
        result?[1].Idea.Id.Should().Be(id2);
    }
    [Fact]
    public async Task IdeasService_GetFeedentriesPaginatedAsync_IfNotExistReturnsEmptyList()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<Feedentry>>(
            A<string>.Ignored))
            .Returns<List<Feedentry>?>(new());

        // Act
        var result = await _service.GetFeedentriesPaginatedAsync(1, 10);

        // Assert
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
    [Fact]
    public async Task IdeasService_GetFeedentriesPaginatedAsync_IfBaseNullThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<List<Feedentry>>(
            A<string>.Ignored))
            .Returns<List<Feedentry>?>(null);

        // Act
        Func<Task> act = async () => await _service.GetFeedentriesPaginatedAsync(1);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task IdeasService_PostIdeaAsync_NoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<IdeaToDb, IdeaFromDb>(
            A<string>.Ignored, A<IdeaToDb>.Ignored))
            .Returns(new IdeaFromDb());

        // Act
        Func<Task> act = async () => await _service.PostIdeaAsync(new());

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
}
