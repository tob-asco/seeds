using FakeItEasy.Sdk;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System.Net;
using System.Net.Http.Json;

namespace seeds.Dal.Tests.Services;

public class DalBaseServiceTests
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly DalBaseService _service;
    public DalBaseServiceTests()
    {
        _httpClientWrapper = A.Fake<IHttpClientWrapper>();
        _service = new(_httpClientWrapper);
    }

    [Fact]
    public async Task DalBaseService_GetDalModelAsync_ReturnsUser()
    {
        // Arrange
        string username = "dummy";
        UserDto user = new() { Username = username };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(user)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        string url = "";

        // Act
        var result = await _service.GetDalModelAsync<UserDto>(url);

        // Assert
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
    }
    [Theory]
    [InlineData(typeof(UserDto))]
    [InlineData(typeof(IdeaFromDb))]
    [InlineData(typeof(CategoryDto))]
    [InlineData(typeof(UserPreference))]
    [InlineData(typeof(UserIdeaInteraction))]
    public async Task DalBaseService_GetDalModelAsync_IfNotFoundWithCorrectHeaderReturnsNullOnDalModel(
        Type T)
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        };
        response.Headers.Add("X-Error-Type", "DbRecordNotFound");
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        // Create an instance of GetDalModelAsyncMethodInfo with the generic parameter T
        var getDalModelAsyncMethodInfo = typeof(DalBaseService)
            .GetMethod("GetDalModelAsync")?
            .MakeGenericMethod(T);
        string url = "";

        // Act
        Task? task = getDalModelAsyncMethodInfo?
            .Invoke(_service, new object[] { url }) as Task;
        if (task != null) await task.ConfigureAwait(false);
        //Get the result using reflection
        var result = getDalModelAsyncMethodInfo?.ReturnType
            .GetProperty("Result")?.GetValue(task);

        // Assert
        result.Should().BeNull(because: "higher layers rely on Null meaning 404");
    }
    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public async Task DalBaseService_GetDalModelAsync_IfErrorWithoutHeaderThrows(
        HttpStatusCode code)
    {
        /* Important behaviour because we use GetDalModelAsync() also
         * on datatypes that do not have null as default.
         * E.g. int has default(int)=0 and it would be very bad to interpret
         * this 0 as an actual count result.
         */
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = code,
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act1 = async () => await _service.GetDalModelAsync<UserDto>("");
        Func<Task> act2 = async () => await _service.GetDalModelAsync<int>("");

        // Assert
        await act1.Should().ThrowAsync<Exception>();
        await act2.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task DalBaseService_PutDalModelAsync_ReturnsTrue()
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Accepted,
        };
        A.CallTo(() => _httpClientWrapper.PutAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.PutDalModelAsync(url, user);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public async Task DalBaseService_PutDalModelAsync_IfNotFoundReturnsFalse()
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        };
        A.CallTo(() => _httpClientWrapper.PutAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.PutDalModelAsync(url, user);

        // Assert
        result.Should().BeFalse();
    }
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.Forbidden)]
    public async Task DalBaseService_PutDalModelAsync_IfNon404ErrorThrows(
        HttpStatusCode code)
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = code,
        };
        A.CallTo(() => _httpClientWrapper.PutAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () => await _service.PutDalModelAsync(url, user);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task DalBaseService_PostDalModelBoolReturnAsync_ReturnsTrue()
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.PostDalModelBoolReturnAsync(url, user);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public async Task DalBaseService_PostDalModelBoolReturnAsync_IfConflictReturnsFalse()
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict,
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.PostDalModelBoolReturnAsync(url, user);

        // Assert
        result.Should().BeFalse();
    }
    [Theory]
    [InlineData(HttpStatusCode.BadRequest)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.Forbidden)]
    public async Task DalBaseService_PostDalModelBoolReturnAsync_IfNonConflictErrorThrows(
        HttpStatusCode code)
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = code,
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () => await _service.PostDalModelBoolReturnAsync(url, user);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task DalBaseService_PostDalModelAsync_NoExceptionOnIdea()
    {
        // Arrange
        IdeaFromDb ideaFromDb = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = JsonContent.Create(ideaFromDb)
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () =>
            await _service.PostDalModelAsync<IdeaToDb, IdeaFromDb>("", new());

        // Assert
        await act.Should().NotThrowAsync<Exception>();
    }
    [Fact]
    public async Task DalBaseService_PostDalModelAsync_IfErrorThrows()
    {
        // Arrange
        IdeaFromDb ideaFromDb = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict,
            Content = JsonContent.Create(ideaFromDb)
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () =>
            await _service.PostDalModelAsync<IdeaToDb, IdeaFromDb>("", new());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task DalBaseService_PostDalModelAsync_IfReturnsNullThrows()
    {
        // Arrange
        IdeaFromDb ideaFromDb = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Created,
            Content = null!
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () =>
            await _service.PostDalModelAsync<IdeaToDb, IdeaFromDb>("", new());

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Theory]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public async Task DalBaseService_DeleteAsync_IfNon404ErrorThrows(
        HttpStatusCode code)
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = code,
        };
        A.CallTo(() => _httpClientWrapper.DeleteAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () => await _service.DeleteAsync(url);

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
    [Fact]
    public async Task DalBaseService_DeleteAsync_ReturnsTrue()
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
        };
        A.CallTo(() => _httpClientWrapper.DeleteAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.DeleteAsync(url);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public async Task DalBaseService_DeleteAsync_IfNotFoundReturnsFalse()
    {
        // Arrange
        string url = "";
        UserDto user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        };
        A.CallTo(() => _httpClientWrapper.DeleteAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.DeleteAsync(url);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task DalBaseService_GetNonDalModelAsync_ReturnsInt()
    {
        // Arrange
        int someInt = 8;
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(someInt)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.GetDalModelAsync<int>("");

        // Assert
        result.Should().Be(someInt);
    }
    [Theory]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public async Task DalBaseService_GetNonDalModelAsync_IfBadResponseThrows(
        HttpStatusCode code)
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = code,
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act = async () => await _service.GetNonDalModelAsync<int>("");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
