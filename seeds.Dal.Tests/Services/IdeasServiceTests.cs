using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
        IdeaDtoApi idea = new() { Id = id, Title = "title", };
        A.CallTo(() => _baseService.GetDalModelAsync<IdeaDtoApi>(A<string>.Ignored))
            .Returns(idea);

        // Act
        var result = await _service.GetIdeaAsync(id);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(id);
    }
    [Fact]
    public async Task IdeasService_GetIdeaAsync_IfNotExistReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<IdeaDtoApi>(
            A<string>.Ignored))
            .Returns<IdeaDtoApi?>(null);

        // Act
        var result = await _service.GetIdeaAsync(1); //"1" doesn't matter

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public async void IdeasService_GetIdeasPaginatedAsync_ReturnsAllItselfs()
    {
        #region Arrange
        int page = 2; int maxPageSize = 10;
        int id1 = 1; int id2 = 2;
        List<IdeaDtoApi> users = new()
        {
            new IdeaDtoApi{ Id = id1, Title = "1st Idea" },
            new IdeaDtoApi{ Id = id2, Title = "2nd Idea" },
        };
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaDtoApi>>(A<string>.Ignored))
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
        A.CallTo(() => _baseService.GetDalModelAsync<List<IdeaDtoApi>>(
            A<string>.Ignored))
            .Returns<List<IdeaDtoApi>?>(null);

        // Act
        var result = await _service.GetIdeasPaginatedAsync(1, 10);

        // Assert
        result.Should().BeNull();
    }
}
