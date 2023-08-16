using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System.Net;

namespace seeds.Dal.Tests.Services;

public class UserPreferenceServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly UserPreferenceService _service;
    public UserPreferenceServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new UserPreferenceService(_baseService);
    }
    [Fact]
    public async Task CupService_UpsertUserPreference_NoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelBoolReturnAsync(
            A<string>.Ignored, A<UserPreference>.Ignored))
            .Returns(true);

        // Act
        Func<Task> act = async () =>
            await _service.UpsertUserPreferenceAsync("", new Guid(), 0);

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
    [Fact]
    public async Task CupService_UpsertUserPreference_IfNotSuccessThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync(
            A<string>.Ignored, A<UserPreference>.Ignored))
            .Returns(false);

        // Act
        Func<Task> act = async () =>
            await _service.UpsertUserPreferenceAsync("", new Guid(), 0);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
