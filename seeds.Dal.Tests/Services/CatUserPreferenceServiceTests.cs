using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System.Net;

namespace seeds.Dal.Tests.Services;

public class CatagUserPreferenceServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly UserPreferenceService _service;
    public CatagUserPreferenceServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new UserPreferenceService(_baseService);
    }
    [Fact]
    public async Task CupService_GetCupAsync_ReturnsItself()
    {
        #region Arrange
        string key = "ABC";
        string uname = "dude";
        UserPreference cup = new()
        {
            CategoryKey = key,
            Username = uname,
            Value = 1
        };
        A.CallTo(() => _baseService.GetDalModelAsync<UserPreference>(
            A<string>.Ignored))
            .Returns(cup);
        #endregion

        // Act
        var result = await _service.GetCatagUserPreferenceAsync(key, uname, null);

        // Assert
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(key);
        result?.Username.Should().Be(uname);
        result?.TagName.Should().BeNull();
    }
    [Fact]
    public async Task CupService_GetCupAsync_IfNotExistThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<UserPreference>(
            A<string>.Ignored))
            .Returns<UserPreference?>(null);

        // Act
        Func<Task> act = async () => await _service.GetCatagUserPreferenceAsync("","","");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task CupService_PutCupAsync_ReturnsTrue()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<UserPreference>(
            A<string>.Ignored, A<UserPreference>.Ignored))
            .Returns(true);
        string key = "ABC";
        string uname = "dude";
        int newVal = -1;

        // Act
        var result = await _service.PutCatagUserPreferenceAsync(key, uname, newVal, null);

        // Assert
        result.Should().Be(true);
    }
    [Fact]
    public async Task CupService_PutCupAsync_IfNotSuccessReturnsFalse()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<UserPreference>(
            A<string>.Ignored, A<UserPreference>.Ignored))
            .Returns(false);
        string key = "ABC";
        string uname = "dude";
        int newVal = -1;

        // Act
        var result = await _service.PutCatagUserPreferenceAsync(key, uname, newVal, null);

        // Assert
        result.Should().Be(false);
    }
}
