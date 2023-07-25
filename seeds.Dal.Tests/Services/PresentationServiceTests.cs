using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds.Dal.Tests.Services;

public class PresentationServiceTests
{
    private readonly IDalBaseService _baseService;
    private readonly PresentationService _service;
    public PresentationServiceTests()
    {
        _baseService = A.Fake<IDalBaseService>();
        _service = new(_baseService);
    }
    [Fact]
    public async Task PresentationService_GetPresentationByIdeaIdAsync_ReturnsItself()
    {
        #region Arrange
        int id = 7;
        Presentation presi = new() { IdeaId = id };
        A.CallTo(() => _baseService.GetDalModelAsync<Presentation>(
            A<string>.Ignored))
            .Returns(presi);
        #endregion

        // Act
        var result = await _service.GetPresentationByIdeaIdAsync(id);

        // Assert
        result.Should().NotBeNull();
        result?.IdeaId.Should().Be(id);
    }
    [Fact]
    public async Task PresentationService_GetPresentationByIdeaIdAsync_IfNotExistReturnsNull()
    {
        // Arrange
        A.CallTo(() => _baseService.GetDalModelAsync<Presentation>(
            A<string>.Ignored))
            .Returns<Presentation?>(null);

        // Act
        var result = await _service.GetPresentationByIdeaIdAsync(0);

        // Assert
        result.Should().BeNull();
    }
    [Fact]
    public async Task PresentationService_PutPresentationByIdeaIdAsync_ReturnsTrue()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(true);
        Presentation presi = new();

        // Act
        var result = await _service.PutPresentationByIdeaIdAsync(presi.IdeaId, presi);

        // Assert
        result.Should().Be(true);
    }
    [Fact]
    public async Task PresentationService_PutPresentationAsync_IfNotSuccessReturnsFalse()
    {
        // Arrange
        A.CallTo(() => _baseService.PutDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(false);
        Presentation presi = new();

        // Act
        var result = await _service.PutPresentationByIdeaIdAsync(presi.IdeaId, presi);

        // Assert
        result.Should().Be(false);
    }
    [Fact]
    public async Task PresentationService_PostPresentationAsync_ReturnsTrue()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(true);

        // Act
        var result = await _service.PostPresentationAsync(new());

        // Assert
        result.Should().Be(true);
    }
    [Fact]
    public async Task PresentationService_PostPresentationAsync_IfNotSuccessReturnsFalse()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(false);

        // Act
        var result = await _service.PostPresentationAsync(new());

        // Assert
        result.Should().Be(false);
    }
    [Fact]
    public async Task PresentationService_PostOrPutPresentationAsync_IfExistNoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.PutDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(true);

        // Act
        Func<Task> act = async () => await _service.PostOrPutPresentationAsync(new());

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task PresentationService_PostOrPutPresentationAsync_IfNotFoundNoException()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(true);
        A.CallTo(() => _baseService.PutDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.PostOrPutPresentationAsync(new());

        // Assert
        await act.Should().NotThrowAsync();
    }
    [Fact]
    public async Task PresentationService_PostOrPutPresentationAsync_IfErrorThrows()
    {
        // Arrange
        A.CallTo(() => _baseService.PostDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(false);
        A.CallTo(() => _baseService.PutDalModelAsync<Presentation>(
            A<string>.Ignored, A<Presentation>.Ignored))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _service.PostOrPutPresentationAsync(new());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
