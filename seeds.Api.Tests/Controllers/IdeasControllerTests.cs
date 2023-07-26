using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class IdeasControllerTests : ApiBaseControllerTests
{

    public List<Idea> Ideas { get; set; } = new();

    public IdeasControllerTests()
    {
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 22; i++)
        {
            Ideas.Add(
            new Idea()
            {
                Title = "Idea #" + i
            });
        }
        if (!_context.Idea.Any()) { _context.Idea.AddRange(Ideas); }
    }

    [Theory]
    [InlineData(1, 10)] // MaxPageSize length
    [InlineData(4, 7)] // At least one
    [InlineData(5, 10)] // None
    public async Task IdeasController_GetPaginatedEndpoint_ReturnsListOfCorrectSizeOrNotFound(
        int page, int maxPageSize)
    {
        //Arrange
        string url = $"api/Ideas/page/{page}/size/{maxPageSize}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        if (Ideas.Count >= (page * maxPageSize))
        {
            response.Should().BeSuccessful();
            var result = await response.Content.ReadFromJsonAsync<List<IdeaFromDb>>();
            result.Should().HaveCount(maxPageSize);
        }
        else if (Ideas.Count < (page * maxPageSize) &&
            Ideas.Count > ((page - 1) * maxPageSize))
        {
            response.Should().BeSuccessful();
            var result = await response.Content.ReadFromJsonAsync<List<IdeaFromDb>>();
            result.Should().HaveCount(Ideas.Count - ((page - 1) * maxPageSize));
        }
        else
        {
            response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        }
    }
    [Fact]
    public async Task IdeasController_GetIdeaEndpoint_ReturnsIdeaDto()
    {
        //Arrange
        int id = Ideas[0].Id;
        string url = $"/api/Ideas/{id}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<IdeaFromDb>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Id.Should().Be(id);

    }
    [Fact]
    public async Task IdeasController_GetIdeaEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int id = -900;
        string url = $"/api/Ideas/{id}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task IdeasController_PutIdeaEndpoint_ReturnsSuccess()
    {
        //Arrange
        int id = Ideas[0].Id;
        IdeaFromDb idea = new()
        {
            Id = id,
            Title = "new title"
        };
        string url = $"/api/Ideas/{id}";
        var content = JsonContent.Create(idea);

        //Act
        var response = await _httpClient.PutAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
    }
    [Fact]
    public async Task IdeasController_PutIdeaEndpoint_UpdatesTitleAndLeavesIdAndCreationTime()
    {
        //Arrange
        int index = 0;
        int id = Ideas[index].Id;
        DateTime time = DateTime.MinValue;
        IdeaFromDb idea = new()
        {
            Id = id,
            CreationTime = time,
            Title = Guid.NewGuid().ToString(),
        };
        string url = $"/api/Ideas/{id}";
        var content = JsonContent.Create(idea);

        //Act
        await _httpClient.PutAsync(url, content);

        //Assert
        _context.Idea.Find(id).Should().NotBeNull();
        _context.Idea.Find(id)?.CreationTime.Should().Be(time);
        _context.Idea.Find(id)?.Title.Should().NotBe(Ideas[index].Title);
    }
    [Fact]
    public async Task IdeasController_PutIdeaEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int id = -900;
        string url = $"/api/Ideas/{id}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task IdeasController_PostIdeaEndpoint_ReturnsSuccess()
    {
        //Arrange
        IdeaToDb idea = new()
        {
            Title = "new title",
        };
        string url = $"/api/Ideas";
        var content = JsonContent.Create(idea);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
    }
    [Fact]
    public async Task IdeasController_PostIdeaEndpoint_ReturnsIdeaWithPkFromDb()
    {
        //Arrange
        string title = Guid.NewGuid().ToString();
        IdeaToDb idea = new()
        {
            Title = title,
        };
        string url = $"/api/Ideas";
        var content = JsonContent.Create(idea);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        var result = await response.Content.ReadFromJsonAsync<Idea>();
        result.Should().NotBeNull();
        result?.Id.Should().Be(
            _context.Idea.FirstOrDefault(i=>i.Title == title)!.Id);
    }
}
