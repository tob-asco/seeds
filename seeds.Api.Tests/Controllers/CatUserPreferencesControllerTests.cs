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
    public List<CatagUserPreference> Cups { get; set; } = new();

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
    public async Task CupController_GetEndpoint_ForNoTagReturnsItself()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(key)}/{HttpUtility.UrlEncode(username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<CatagUserPreference>();
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
        result?.CategoryKey.Should().Be(key);
    }
    [Fact]
    public async Task CupController_GetEndpoint_ForTagReturnsItself()
    {
        //Arrange
        string key = Cats[catIndexWithTag].Key;
        string username = Users[2].Username;
        string tagname = Tag.Name;
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(key)}/{HttpUtility.UrlEncode(username)}" +
            $"?tagName={HttpUtility.UrlEncode(tagname)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<CatagUserPreference>();
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
        result?.CategoryKey.Should().Be(key);
        result?.TagName.Should().Be(tagname);
    }
    [Fact]
    public async Task CupController_GetEndpoint_IfCatNotExistReturnsNotFound()
    {
        //Arrange
        string url = $"api/CatagUserPreferences/BlöDeCat/" +
            $"{HttpUtility.UrlEncode(Users[0].Username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CupController_GetEndpoint_IfUsernameNotExistReturnsNotFound()
    {
        //Arrange
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(Cats[0].Name)}/notUser";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CupController_GetEndpoint_IfTagNotExistReturnsNotFound()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;
        string urlSuccess = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(key)}/{HttpUtility.UrlEncode(username)}";
        string url = $"{urlSuccess}?tagName=notATAG";

        //Act
        var responseSuccess = await _httpClient.GetAsync(urlSuccess);
        var response = await _httpClient.GetAsync(url);

        //Assert
        responseSuccess.Should().HaveStatusCode(HttpStatusCode.OK);
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CupController_PutEndpoint_ForNoTagReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        CatagUserPreference cup = Cups[^1];
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
        CatagUserPreference cup = Cups.FirstOrDefault(cup => cup.TagName != null)!;
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
        CatagUserPreference cup = new() { CategoryKey = key, Username = uname };

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
        CatagUserPreference cup = new() { CategoryKey = key, Username = uname };

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
        CatagUserPreference cup = new()
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
