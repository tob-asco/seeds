using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using seeds.Api.Controllers;
using seeds.Api.Data;
using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Api.Tests.Controllers;

public class CatUserPreferencesControllerTests
{
    private readonly CategoryUserPreferencesController _controller;
    private readonly HttpClient _httpClient;

    public List<User> Users { get; set; } = new();
    public List<Category> Cats { get; set; } = new();
    public List<CategoryUserPreference> Cups { get; set; } = new();

    public CatUserPreferencesControllerTests()
    {
        var context = CreateDb();
        _controller = new CategoryUserPreferencesController(context);

        // Create the HttpClient using the in-memory server
        var factory = new WebApplicationFactory<ProgramTest>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(context);
                });
            });
        _httpClient = factory.CreateClient();
    }

    private seedsApiContext CreateDb()
    {
        var options = new DbContextOptionsBuilder<seedsApiContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var context = new seedsApiContext(options);
        context.Database.EnsureCreated();
        if (!(context.CategoryUserPreference.Any()))
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
            context.Category.AddRange(Cats);
            context.User.AddRange(Users);
            foreach(var cat in Cats)
            {
                foreach(var user in Users)
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
            context.CategoryUserPreference.AddRange(Cups);
            context.SaveChanges();
        }
        return context;
    }

    #region Unit Testing

    [Fact]
    public async Task CatUserPrefencesController_GetCatUserPreferenceAsync_ReturnsItself()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;

        //Act
        var result = await _controller.GetCategoryUserPreferenceAsync(key,username);

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

    #endregion
    #region Enpoint Testing

    [Fact]
    public async Task CatUserPrefencesController_GetEndpoint_ReturnsNotNull()
    {
        //Arrange
        string key = Cats[3].Key;
        string username = Users[2].Username;
        CategoryUserPreference cup = new()
        {
            CategoryKey = key,
            Username = username,
            Value = 8
        };
        string url = $"/api/CategoryUserPreferences/{key}/{username}";

        //Act
        var response = await _httpClient.GetAsync(url);
        var result = await response.Content.ReadFromJsonAsync<CategoryUserPreference>();

        //Assert
        response.IsSuccessStatusCode.Should().Be(true);
        result.Should().NotBeNull();
        result.CategoryKey.Should().Be(key);
        result.Username.Should().Be(username);
    }

    #endregion
}
