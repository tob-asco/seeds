using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class UserPreferencesControllerTests : ApiControllerTestsBase
{
    public List<User> Users { get; set; } = new();
    public List<Tag> Tags { get; set; } = new();
    public Category Category { get; set; } = new();
    public List<UserPreference> Cups { get; set; } = new();
    readonly int tagsIndexWithoutCup = 10;

    public UserPreferencesControllerTests()
    {
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        if (!_context.Category.Any()) { _context.Category.Add(Category); }
        for (int i = 0; i <= tagsIndexWithoutCup; i++)
        {
            Tags.Add(new()
            {
                CategoryKey = Category.Key,
                Name = $"tag !{i}"
            });
            Users.Add(new()
            {
                Username = $"t??i{i}", //unique
                Password = "tobi",
                Email = "tobi" + i + "@tobi.com", //unique
            });
        }
        if (!_context.Tag.Any()) { _context.Tag.AddRange(Tags); }
        if (!_context.User.Any()) { _context.User.AddRange(Users); }
        _context.SaveChanges();
        foreach (var user in Users)
        {
            // last tag has no CUP
            foreach (var tag in Tags.Take(Tags.Count - 1))
            {
                Cups.Add(new()
                {
                    ItemId = _context.Tag.FirstOrDefault(t =>
                        t.CategoryKey == tag.CategoryKey &&
                        t.Name == tag.Name)!.Id,
                    Username = user.Username,
                    Value = 1,
                });
            }
        }
        if (!_context.UserPreference.Any())
        {
            _context.UserPreference.AddRange(Cups);
        }
    }


    [Fact]
    public async Task CupController_GetUpOfUserEndpoint_ReturnsItselfs()
    {
        //Arrange
        string username = Users[0].Username;
        string url = $"api/UserPreferences/{username}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<UserPreference>>();
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_context.UserPreference.Where(
            cup => cup.Username == username));
    }
    [Fact]
    public async Task CupController_GetUpOfUserEndpoint_IfUserNotExistsReturnsEmpty()
    {
        //Arrange
        string url = $"api/UserPreferences/notAuser";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<UserPreference>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }
    [Fact]
    public async Task CupController_PostOrPutEndpoint_ForPostReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        UserPreference cup = new()
        {
            ItemId = _context.Tag.First(t =>
                t.CategoryKey == Tags[tagsIndexWithoutCup].CategoryKey &&
                t.Name == Tags[tagsIndexWithoutCup].Name).Id,
            Username = Users[0].Username,
            Value = 1
        };
        string url = $"api/UserPreferences/upsert";
        var content = JsonContent.Create(cup);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
        _context.UserPreference.Should().ContainEquivalentOf(cup);
    }
    [Fact]
    public async Task CupController_PostOrPutEndpoint_ForPutReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        int index = 0;
        UserPreference cup = Cups[index];
        cup.Value++;
        string url = $"api/UserPreferences/upsert";
        var content = JsonContent.Create(cup);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
        _context.UserPreference.Should().ContainEquivalentOf(cup);
        _context.UserPreference.Should().NotContainEquivalentOf(Cups[index]);
    }
    [Fact]
    public async Task CupController_PostOrPutEndpoint_IfTagNotExistReturnsNotSuccess()
    {
        //Arrange
        string url = $"/api/UserPreferences/upsert";
        UserPreference cup = new()
        {
            ItemId = Guid.NewGuid(),
            Username = Users[0].Username
        };

        //Act
        var response = await _httpClient.PutAsync(url, JsonContent.Create(cup));

        //Assert
        response.IsSuccessStatusCode.Should().BeFalse();
    }
}
