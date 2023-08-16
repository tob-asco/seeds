﻿using Microsoft.AspNetCore.Mvc;
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
        if (!_context.CatagUserPreference.Any())
        {
            _context.CatagUserPreference.AddRange(Cups);
        }
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
        _context.CatagUserPreference.Should().ContainEquivalentOf(cup);
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
        _context.CatagUserPreference.Should().ContainEquivalentOf(cup);
        _context.CatagUserPreference.Should().NotContainEquivalentOf(Cups[index]);
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
