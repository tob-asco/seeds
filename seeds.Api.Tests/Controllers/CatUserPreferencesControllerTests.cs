using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Api.Tests.Controllers;

public class CatUserPreferencesControllerTests : ApiBaseControllerTests
{
    private readonly CategoryUserPreferencesController _controller;
    public List<User> Users { get; set; } = new();
    public List<Category> Cats { get; set; } = new();
    public List<CategoryUserPreference> Cups { get; set; } = new();

    public CatUserPreferencesControllerTests()
    {
        _controller = new(_context);
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
                Key = $"Cat{i}",
                Name = $"Category{i}"
            });
            Users.Add(new()
            {
                Username = $"tobi{i}", //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
        }
        if (!_context.Category.Any()) { _context.Category.AddRange(Cats); }
        if (!_context.User.Any()) { _context.User.AddRange(Users); }
        foreach (var cat in Cats)
        {
            foreach (var user in Users)
            {
                Cups.Add(
                new CategoryUserPreference()
                {
                    CategoryKey = cat.Key,
                    Username = user.Username,
                    Value = (Cups.Count % 3) - 1
                });
            }
        }
        if (!_context.CategoryUserPreference.Any())
        {
            _context.CategoryUserPreference.AddRange(Cups);
        }
    }

    [Fact]
    public async Task CupController_GetEndpoint_ReturnsItself()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;
        string url = $"api/CategoryUserPreferences/{key}/{username}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<CategoryUserPreference>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
        result?.CategoryKey.Should().Be(key);
    }
    [Fact]
    public async Task CupController_GetEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        string url = $"api/CategoryUserPreferences/BlöDeCat/notUser";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
    [Fact]
    public async Task CupController_PutEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        string key = Cats[4].Key;
        string username = Users[1].Username;
        CategoryUserPreference cup = new()
        {
            CategoryKey = key,
            Username = username,
            Value = 1798
        };
        string url = $"/api/CategoryUserPreferences/{key}/{username}";
        var content = JsonContent.Create(cup);

        //Act
        var response = await _httpClient.PutAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
        _context.CategoryUserPreference.Should().ContainEquivalentOf(cup);
    }
    [Fact]
    public async Task CupController_PutEndpoint_IfNotExistReturnsNotFound()
    {
        //Arrange
        string key = "NotACat";
        string username = "noUser";
        string url = $"/api/CategoryUserPreferences/{key}/{username}";
        CategoryUserPreference cup = new()
        {
            CategoryKey = key,
            Username = username,
            Value = -80,
        };

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(cup));

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
    }
}
