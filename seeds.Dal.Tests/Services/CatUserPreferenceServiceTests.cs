using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System.Net;

namespace seeds.Dal.Tests.Services;

public class CatUserPreferenceServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly CategoryUserPreferenceService _service;
    public CatUserPreferenceServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new CategoryUserPreferenceService(_baseService);
    }
    [Fact]
    public async Task CupService_GetCupAsync_ReturnsItself()
    {
        #region Arrange
        string key = "ABC";
        string uname = "dude";
        CategoryUserPreference cup = new()
        {
            CategoryKey = key,
            Username = uname,
            Value = 1
        };
        A.CallTo(() => _baseService.GetDalModelAsync<CategoryUserPreference>(
            A<string>.Ignored))
            .Returns(cup);
        #endregion

        // Act
        var result = await _service.GetCategoryUserPreferenceAsync(key, uname);

        // Assert
        result.Should().NotBeNull();
        result?.CategoryKey.Should().Be(key);
        result?.Username.Should().Be(uname);
    }
    [Fact]
    public async Task CupService_GetCupAsync_IfNotExistThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<CategoryUserPreference>(
            A<string>.Ignored))
            .Returns<CategoryUserPreference?>(null);

        // Act
        Func<Task> act = async () => await _service.GetCategoryUserPreferenceAsync("N0C","");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task CupService_PutCupAsync_ReturnsTrue()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<CategoryUserPreference>(
            A<string>.Ignored, A<CategoryUserPreference>.Ignored))
            .Returns(true);
        string key = "ABC";
        string uname = "dude";
        int newVal = -1;

        // Act
        var result = await _service.PutCategoryUserPreferenceAsync(key, uname, newVal);

        // Assert
        result.Should().Be(true);
    }
    [Fact]
    public async Task CupService_PutCupAsync_IfNotSuccessReturnsFalse()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<CategoryUserPreference>(
            A<string>.Ignored, A<CategoryUserPreference>.Ignored))
            .Returns(false);
        string key = "ABC";
        string uname = "dude";
        int newVal = -1;

        // Act
        var result = await _service.PutCategoryUserPreferenceAsync(key, uname, newVal);

        // Assert
        result.Should().Be(false);
    }
}
