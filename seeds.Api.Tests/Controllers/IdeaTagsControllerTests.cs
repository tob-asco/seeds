using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class IdeaTagsControllerTests : ApiBaseControllerTests
{
    private readonly IdeaTagsController _controller;
    public List<Idea> Ideas { get; } = new();
    public List<Tag> Tags { get; } = new();
    public List<IdeaTag> IdeaTags { get; } = new();
    private readonly int indexForIdeaWith3IdeaTags = 0;
    private readonly int indexForIdeasAndTagsWithIdeaTag = 3;
    private readonly int indexForIdeasAndTagsWithoutIdeaTag = 4;

    public IdeaTagsControllerTests()
    {
        _controller = new(_context);
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 22; i++)
        {
            Ideas.Add(new()
            {
                Title = "Idea #" + i
            });
            Tags.Add(new()
            {
                CategoryKey = "NoC",
                Name = "tag #" + i
            });
        }
        if (!_context.Idea.Any()) { _context.Idea.AddRange(Ideas); }
        if (!_context.Tag.Any()) { _context.Tag.AddRange(Tags); }
        for (int i = 0; i <= 2; i++)
        {
            IdeaTags.Add(new()
            {
                IdeaId = Ideas[indexForIdeaWith3IdeaTags].Id,
                CategoryKey = Tags[i].CategoryKey,
                TagName = Tags[i].Name
            });
        }
        IdeaTags.Add(new()
        {
            IdeaId = Ideas[indexForIdeasAndTagsWithIdeaTag].Id,
            CategoryKey = Tags[indexForIdeasAndTagsWithIdeaTag].CategoryKey,
            TagName = Tags[indexForIdeasAndTagsWithIdeaTag].Name
        });
        if (!_context.IdeaTag.Any()) { _context.IdeaTag.AddRange(IdeaTags); }
        var ideaTag = _context.IdeaTag.Find(
            Ideas[indexForIdeasAndTagsWithoutIdeaTag].Id,
            Tags[indexForIdeasAndTagsWithoutIdeaTag].CategoryKey,
            Tags[indexForIdeasAndTagsWithoutIdeaTag].Name);
        if (ideaTag != null) { _context.IdeaTag.Remove(ideaTag); }
    }

    [Fact]
    public async Task IdeaTagsController_GetTagsOfIdeaEndpoint_Returns3Items()
    {
        //Arrange
        int ideaId = Ideas[indexForIdeaWith3IdeaTags].Id;
        string url = $"/api/IdeaTags/{ideaId}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<IdeaTag>>();
        result.Should().NotBeNull();
        result?.Should().HaveCount(3);
    }
    [Fact]
    public async Task IdeaTagsController_GetTagsOfIdeaEndpoint_IfNoTagReturnsEmptyList()
    {
        // Arrange
        int ideaId = Ideas[indexForIdeasAndTagsWithoutIdeaTag].Id;
        string url = $"/api/IdeaTags/{ideaId}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<IdeaTag>>();
        result.Should().NotBeNull();
        result?.Should().HaveCount(0);
    }
    [Fact]
    public async Task IdeaTagsController_PostEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        IdeaTag ideaTag = new()
        {
            IdeaId = Ideas[indexForIdeasAndTagsWithoutIdeaTag].Id,
            CategoryKey = Tags[indexForIdeasAndTagsWithoutIdeaTag].CategoryKey,
            TagName = Tags[indexForIdeasAndTagsWithoutIdeaTag].Name
        };
        string url = "api/IdeaTags";

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(ideaTag));

        //Assert
        response.Should().BeSuccessful();
        _context.IdeaTag.Should().ContainEquivalentOf(ideaTag);
    }
    [Fact]
    public async Task IdeaTagsController_PostEndpoint_IfExistReturnsConflict()
    {
        //Arrange
        IdeaTag ideaTag = new()
        {
            IdeaId = Ideas[indexForIdeasAndTagsWithIdeaTag].Id,
            CategoryKey = Tags[indexForIdeasAndTagsWithIdeaTag].CategoryKey,
            TagName = Tags[indexForIdeasAndTagsWithIdeaTag].Name
        };
        string url = "api/IdeaTags";

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(ideaTag));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }
    [Fact]
    public async Task IdeaTagsController_DeleteEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        int ideaId = Ideas[indexForIdeasAndTagsWithIdeaTag].Id;
        string catKey = Tags[indexForIdeasAndTagsWithIdeaTag].CategoryKey;
        string tagName = Tags[indexForIdeasAndTagsWithIdeaTag].Name;
        string url = $"api/IdeaTags/{ideaId}/{catKey}/{tagName}";

        //Act
        var response = await _httpClient.DeleteAsync(url);

        //Assert
        response.Should().BeSuccessful();
        _context.IdeaTag.Should().NotContainEquivalentOf(
            IdeaTags.FirstOrDefault(it =>
                it.IdeaId == Ideas[indexForIdeasAndTagsWithIdeaTag].Id &&
                it.CategoryKey == Tags[indexForIdeasAndTagsWithIdeaTag].CategoryKey &&
                it.TagName == Tags[indexForIdeasAndTagsWithIdeaTag].Name
            )!);
    }
    [Fact]
    public async Task IdeaTagsController_DeleteEndpoint_IfNotExistReturnsNotFound()
    {
        // Arrange
        int ideaId = Ideas[indexForIdeasAndTagsWithoutIdeaTag].Id;
        string catKey = Tags[indexForIdeasAndTagsWithoutIdeaTag].CategoryKey;
        string tagName = Tags[indexForIdeasAndTagsWithoutIdeaTag].Name;
        string url = $"/api/IdeaTags/{ideaId}/{catKey}/{tagName}";

        // Act
        var response = await _httpClient.DeleteAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
