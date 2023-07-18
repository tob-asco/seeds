using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Dal.Model;
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
    }
    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Cats.Add(
            new Category()
            {
                Key = $"Cat{i}",
                Name = $"Category{i}"
            });
            Users.Add(
            new User()
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


    #region Unit Testing

    [Fact]
    public async Task CatUserPrefencesController_GetCatUserPreferenceAsync_ReturnsItself()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;

        //Act
        var result = await _controller.GetCategoryUserPreferenceAsync(key, username);

        //Assert
        var actionResult = Assert.IsType<ActionResult<CategoryUserPreference>>(result);
        var cup = Assert.IsAssignableFrom<CategoryUserPreference>(actionResult.Value);
        cup.Username.Should().Be(username);
        cup.CategoryKey.Should().Be(key);
    }

    [Fact]
    public async Task CatUserPrefencesController_GetCatUserPreferenceAsync_IfNotExistReturnsNotFound()
    {
        //Arrange
        string key = "BLö";
        string username = Users[2].Username;

        //Act
        var result = await _controller.GetCategoryUserPreferenceAsync(key, username);

        //Assert
        var actionResult = Assert.IsType<ActionResult<CategoryUserPreference>>(result);
        actionResult.Result.Should().BeOfType<NotFoundResult>();
    }

    //not working "entity with key value pair already being tracked" error
    //[Fact]
    //public async Task CatUserPrefencesController_PutCatUserPreferenceAsync_ReturnsSuccess()
    //{
    //    //Arrange
    //    string key = Cats[3].Key;
    //    string username = Users[2].Username;
    //    int val = 18;
    //    CategoryUserPreference cup = new()
    //    {
    //        CategoryKey = key,
    //        Username = username,
    //        Value = val
    //    };

    //    //Act
    //    var result = await _controller.PutCategoryUserPreferenceAsync(key, username, cup);

    //    //Assert
    //    var actionResult = Assert.IsAssignableFrom<ActionResult>(result);
    //}

    #endregion
    #region Enpoint Testing

    [Fact]
    public async Task CatUserPrefencesController_GetEndpoint_ReturnsItself()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;
        string url = $"/api/CategoryUserPreferences/{key}/{username}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<CategoryUserPreference>();

        //Assert
        response.Should().BeSuccessful();
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(key);
        result?.Username.Should().Be(username);
    }

    [Fact]
    public async Task CatUserPrefencesController_PutEndpoint_UpdatesDb()
    {
        //Arrange
        string key = Cats[4].Key;
        string username = Users[1].Username;
        int val = 18;
        CategoryUserPreference cup = new()
        {
            CategoryKey = key,
            Username = username,
            Value = val
        };
        string url = $"/api/CategoryUserPreferences/{key}/{username}";
        var content = JsonContent.Create(cup);
        _context.ChangeTracker.Clear();

        //Act
        var putResponse = await _httpClient.PutAsync(url, content);
        var getResponse = await _httpClient.GetAsync(url);
        var getResult = await getResponse.Content
            .ReadFromJsonAsync<CategoryUserPreference>();

        //Assert
        putResponse.IsSuccessStatusCode.Should().Be(true);
        getResponse.IsSuccessStatusCode.Should().Be(true);
        getResult.Should().NotBeNull();
        getResult?.Value.Should().Be(val);
    }

    #endregion
}
