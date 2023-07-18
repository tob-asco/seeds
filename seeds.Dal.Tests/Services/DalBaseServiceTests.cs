﻿using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System;
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
        _service = new CategoryService(_httpClientWrapper);
    }

    [Fact]
    public async Task DalBaseService_GetDalModelAsync_ReturnsUser()
    {
        // Arrange
        string username = "dummy";
        User user = new()
        {
            Username = username
        };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(user)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        string url = "";

        // Act
        var result = await _service.GetDalModelAsync<User>(url);

        // Assert
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
    }
    [Theory]
    [InlineData(typeof(User))]
    [InlineData(typeof(Idea))]
    [InlineData(typeof(Category))]
    [InlineData(typeof(CategoryUserPreference))]
    [InlineData(typeof(UserIdeaInteraction))]
    [InlineData(typeof(int))]
    public async Task DalBaseService_GetDalModelAsync_IfNotFoundReturnsNullOnDalModel(Type T)
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        };
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
        if (T == typeof(int)) { result.Should().Be(0); }
        else { result.Should().BeNull(); }
    }
    [Theory]
    [InlineData(HttpStatusCode.Conflict)]
    [InlineData(HttpStatusCode.BadGateway)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.GatewayTimeout)]
    public async Task DalBaseService_GetDalModelAsync_IfNon404ErrorThrows(HttpStatusCode code)
    {
        // Arrange
        var response = new HttpResponseMessage
        {
            StatusCode = code,
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);

        // Act
        Func<Task> act1 = async () => await _service.GetDalModelAsync<User>("");
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
        User user = new();
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
        User user = new();
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
    [Fact]
    public async Task DalBaseService_PostDalModelAsync_ReturnsTrue()
    {
        // Arrange
        string url = "";
        User user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Accepted,
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.PostDalModelAsync(url, user);

        // Assert
        result.Should().BeTrue();
    }
    [Fact]
    public async Task DalBaseService_PostDalModelAsync_IfBadStatusCodeReturnsFalse()
    {
        // Arrange
        string url = "";
        User user = new();
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.Conflict,
        };
        A.CallTo(() => _httpClientWrapper.PostAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);

        // Act
        var result = await _service.PostDalModelAsync(url, user);

        // Assert
        result.Should().BeFalse();
    }
}
