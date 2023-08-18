﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using seeds.Api.Controllers;
using seeds.Api.Helpers;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class UserPreferencesControllerTests : ApiControllerTestsBase
{
    public List<User> Users { get; set; } = new();
    public Family Fam { get; set; } = new();
    readonly int tagsIndexWithFamilyAndCup = 4;
    public List<Tag> Tags { get; set; } = new();
    public Category Category { get; set; } = new();
    public List<UserPreference> Cups { get; set; } = new();
    readonly int tagsIndexWithFamilyButNoCup = 10;
    private readonly IMapper mapper;

    public UserPreferencesControllerTests()
        :base(baseUri: "api/UserPreferences/")
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfiles>());
        mapper = config.CreateMapper();
        PopulatePropertiesAndAddToDb();
        _context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        _context.ChangeTracker.Clear();
    }
    private void PopulatePropertiesAndAddToDb()
    {
        if (!_context.Family.Any()) { _context.Family.Add(Fam); }
        if (!_context.Category.Any()) { _context.Category.Add(Category); }
        for (int i = 0; i <= tagsIndexWithFamilyButNoCup; i++)
        {
            Tags.Add(new()
            {
                CategoryKey = Category.Key,
                Name = $"tag !{i}",
                FamilyId = (i == tagsIndexWithFamilyAndCup ||
                    i == tagsIndexWithFamilyButNoCup) ? Fam.Id : null
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
    public async Task CupController_GetPreferencesOfUserEndpoint_ReturnsItselfs()
    {
        //Arrange
        string username = Users[0].Username;
        string url = baseUri + HttpUtility.UrlEncode(username);

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
    public async Task CupController_GetPreferencesOfUserEndpoint_IfUserNotExistsReturnsEmpty()
    {
        //Arrange
        string url = baseUri + "notAuser";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<UserPreference>>();
        result.Should().NotBeNull();
        result.Should().HaveCount(0);
    }
    [Fact]
    public async Task CupController_GetButtonedTagsEndpoint_ReturnsTagsWithoutFamily()
    {
        //Arrange
        string username = Users[0].Username;
        string url = baseUri + $"buttonedTags?username={HttpUtility.UrlEncode(username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<TagFromDb>>();
        result.Should().NotBeNull();
        foreach(var tag in mapper.Map<List<TagFromDb>>(_context.Tag
            .Where(t => t.FamilyId == null))!)
        {
            result.Should().ContainEquivalentOf(tag);
        }
    }
    [Fact]
    public async Task CupController_GetButtonedTagsEndpoint_ReturnsNoTagWithFamilyAndNoCup()
    {
        //Arrange
        string username = Users[0].Username;
        string url = baseUri + $"buttonedTags?username={HttpUtility.UrlEncode(username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<TagFromDb>>();
        result.Should().NotBeNull();
        result.Should().NotContainEquivalentOf(
            mapper.Map<TagFromDb>(Tags[tagsIndexWithFamilyButNoCup]));
    }
    [Fact]
    public async Task CupController_GetButtonedTagsEndpoint_ReturnsTagWithFamilyAndCup()
    {
        //Arrange
        string username = Users[0].Username;
        string url = baseUri + $"buttonedTags?username={HttpUtility.UrlEncode(username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<TagFromDb>>();
        result.Should().NotBeNull();
        result.Should().ContainEquivalentOf(
            mapper.Map<TagFromDb>(Tags[tagsIndexWithFamilyAndCup]));
    }
    [Fact]
    public async Task CupController_GetButtonedTagsEndpoint_IfNoUserReturnsNoTagWithFamilyAndCup()
    {
        //Arrange
        string url = baseUri + $"buttonedTags?username=NotAuser";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<List<TagFromDb>>();
        result.Should().NotBeNull();
        result.Should().NotContainEquivalentOf(
            mapper.Map<TagFromDb>(Tags[tagsIndexWithFamilyAndCup]));
    }
    [Fact]
    public async Task CupController_PostOrPutEndpoint_ForPostReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        UserPreference cup = new()
        {
            ItemId = _context.Tag.First(t =>
                t.CategoryKey == Tags[tagsIndexWithFamilyButNoCup].CategoryKey &&
                t.Name == Tags[tagsIndexWithFamilyButNoCup].Name).Id,
            Username = Users[0].Username,
            Value = 1
        };
        string url = baseUri + $"upsert";
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
        UserPreference oldCup = _context.UserPreference
            .First(cup => cup.ItemId == Cups[index].ItemId
                       && cup.Username == Cups[index].Username);
        _context.ChangeTracker.Clear();
        oldCup.Value++;
        string url = baseUri + $"upsert";
        var content = JsonContent.Create(oldCup);

        //Act
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().BeSuccessful();
        _context.UserPreference.Should().ContainEquivalentOf(oldCup);
        _context.UserPreference.Should().NotContainEquivalentOf(Cups[index]);
    }
    [Fact]
    public async Task CupController_PostOrPutEndpoint_IfTagNotExistReturnsNotSuccess()
    {
        //Arrange
        string url = baseUri + $"upsert";
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
