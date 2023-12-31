﻿using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class PresentationsControllerTests : ApiControllerTestsBase
{
    private readonly int ideasIndexWithPresentation = 3;
    private readonly int ideasIndexWithNoPresentation = 13;
    public List<Idea> Ideas { get; set; } = new();
    public List<Presentation> Presentations { get; set; } = new();
    public PresentationsControllerTests()
        :base(baseUri: "api/Presentations/")
    {
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 0; i < 20; i++)
        {
            Ideas.Add(new()
            {
                Title = "Idea #" + i
            });
        }
        context.Idea.AddRange(Ideas);
        for (int i = 0; i < 10; i++)
        {
            Presentations.Add(new()
            {
                IdeaId = Ideas[i].Id,
                Description = "Description of idea with Id " + Ideas[i].Id,
            });
        }
        context.Presentation.AddRange(Presentations);
    }

    [Fact]
    public async Task PresentationsController_GetByIdeaIdEndpoint_ReturnsItself()
    {
        //Arrange
        int ideaId = Ideas[ideasIndexWithPresentation].Id;
        string url = baseUri + $"{ideaId}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<Presentation>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.IdeaId.Should().Be(ideaId);
    }
    [Fact]
    public async Task PresentationsController_GetByIdeaIdEndpoint_IfNotExistReturnsNotFoundWithMyHeader()
    {
        // Arrange
        int ideaId = Ideas[ideasIndexWithNoPresentation].Id;
        string url = baseUri + $"{ideaId}";

        // Act
        var response = await _httpClient.GetAsync(url);

        // Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        response.Headers.TryGetValues("X-Error-Type", out var vals).Should().BeTrue();
        vals.Should().NotBeNull();
        vals.Should().Contain(s => s == "DbRecordNotFound");
    }
    [Fact]
    public async Task PresentationsController_PutByIdeaIdEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        int ideaId = Ideas[ideasIndexWithPresentation].Id;
        Presentation presi = new()
        {
            Id = Presentations[
                Presentations.IndexOf(Presentations.Find(p => p.IdeaId == ideaId)!)].Id,
            IdeaId = ideaId,
            Description = "new description",
        };
        string url = baseUri + $"{ideaId}";

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(presi));

        //Assert
        response.Should().BeSuccessful();
        context.Presentation.Should().ContainEquivalentOf(presi);
    }
    [Fact]
    public async Task PresentationsController_PutByIdeaIdEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        int ideaId = Ideas[ideasIndexWithNoPresentation].Id;
        int presiId = 0;
        Presentation uii = new()
        {
            Id = presiId,
            IdeaId = ideaId,
        };
        string url = baseUri + $"{ideaId}";

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(uii));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task PresentationsController_PostEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        Presentation presi = new()
        {
            IdeaId = Ideas[ideasIndexWithNoPresentation].Id,
            Description = "new description"
        };
        string url = baseUri;

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(presi));

        //Assert
        response.Should().BeSuccessful();
        context.Presentation.Should().ContainSingle(
            p => p.IdeaId == Ideas[ideasIndexWithNoPresentation].Id);
    }
    [Fact]
    public async Task PresentationsController_PostEndpoint_IfExistReturnsConflict()
    {
        //Arrange
        Presentation presi = Presentations[0];
        string url = baseUri;

        //Act
        var response = await _httpClient.PostAsync(url, JsonContent.Create(presi));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }
}
