using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class CatagUserPreferencesControllerTests : ApiControllerTestsBase
{
    public List<User> Users { get; set; } = new();
    public List<Category> Cats { get; set; } = new();
    static readonly int catIndexWithTag = 0;
    public Tag Tag { get; set; } = new();
    public List<UserPreference> Cups { get; set; } = new();

    public CatagUserPreferencesControllerTests()
    {
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Cats.Add(new()
            {
                Key = $"Cat #{i}",
                Name = $"Category{i}"
            });
            Users.Add(new()
            {
                Username = $"t??i{i}", //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
        }
        if (!_context.Category.Any()) { _context.Category.AddRange(Cats); }
        if (!_context.User.Any()) { _context.User.AddRange(Users); }
        Tag = new()
        {
            CategoryKey = Cats[catIndexWithTag].Key,
            Name = "tag olé"
        };
        if (!_context.Tag.Any()) { _context.Tag.Add(Tag); }
        foreach (var user in Users)
        {
            Cups.Add(new()
            {
                CategoryKey = Tag.CategoryKey,
                Username = user.Username,
                TagName = Tag.Name,
                Value = 1,
            });
            foreach (var cat in Cats)
            {
                Cups.Add(new()
                {
                    CategoryKey = cat.Key,
                    Username = user.Username,
                    Value = (Cups.Count % 3) - 1
                });
            }
        }
        if (!_context.CatagUserPreference.Any())
        {
            _context.CatagUserPreference.AddRange(Cups);
        }
    }

    [Fact]
    public async Task CupController_PutEndpoint_ForNoTagReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        UserPreference cup = Cups[^1];
        cup.Value++;
        string url = $"/api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(cup.CategoryKey)}/{HttpUtility.UrlEncode(cup.Username)}";
        var content = JsonContent.Create(cup);

        //Act
        var response = await _httpClient.PutAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
        _context.CatagUserPreference.Should().ContainEquivalentOf(cup);
    }
    [Fact]
    public async Task CupController_PutEndpoint_ForTagReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        UserPreference cup = Cups.FirstOrDefault(cup => cup.TagName != null)!;
        cup.Value++;
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(cup.CategoryKey)}/{HttpUtility.UrlEncode(cup.Username)}" +
            $"?tagName={HttpUtility.UrlEncode(cup.TagName)}";
        var content = JsonContent.Create(cup);

        //Act
        var response = await _httpClient.PutAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
        _context.CatagUserPreference.Should().ContainEquivalentOf(cup);
    }
    [Fact]
    public async Task CupController_PutEndpoint_IfCatNotExistReturnsNotFound()
    {
        //Arrange
        string key = Guid.NewGuid().ToString();
        string uname = Users[0].Username;
        string url = $"/api/CatagUserPreferences/{key}/{HttpUtility.UrlEncode(uname)}";
        UserPreference cup = new() { CategoryKey = key, Username = uname };

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(cup));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CupController_PutEndpoint_IfUsernameNotExistReturnsNotFound()
    {
        //Arrange
        string key = Cats[0].Key;
        string uname = Guid.NewGuid().ToString();
        string url = $"/api/CatagUserPreferences/{HttpUtility.UrlEncode(key)}/{uname}";
        UserPreference cup = new() { CategoryKey = key, Username = uname };

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(cup));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CupController_PutEndpoint_IfTagNotExistReturnsNotFound()
    {
        //Arrange
        string key = Cats[0].Key;
        string uname = Users[0].Username;
        string tname = Guid.NewGuid().ToString();
        string url = $"/api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(key)}/{HttpUtility.UrlEncode(uname)}?tagName={tname}";
        UserPreference cup = new()
        {
            CategoryKey = key,
            Username = uname,
            TagName = tname
        };

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(cup));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
