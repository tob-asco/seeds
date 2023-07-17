using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System;
using System.Net;

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

    [Theory]
    [InlineData(typeof(User))]
    public async Task DalBaseService_GetDalModelAsync_IfNotFoundOnDalModelReturnsNull(Type T)
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
            .GetMethod("GetDalModelAsync")
            .MakeGenericMethod(T);
        string url = "";

        // Act
        var task = (Task)getDalModelAsyncMethodInfo.Invoke(_service, new object[] { url });
        await task.ConfigureAwait(false);
        //Get the result using reflection
        var result = getDalModelAsyncMethodInfo.ReturnType.GetProperty("Result")?.GetValue(task);

        //var result = getDalModelAsyncMethodInfo.Invoke(_service, new object[] { url });
        //var r = await _service.GetDalModelAsync<User>("");

        // Assert
        //r.Should().BeNull();
        result.Should().BeNull();
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
        Func<Task> act = async () => await _service.GetDalModelAsync<User>("");

        // Assert
        await act.Should().ThrowAsync<Exception>();
    }
}
