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
