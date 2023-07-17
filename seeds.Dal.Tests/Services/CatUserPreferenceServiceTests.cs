using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Tests.Services;

public class CatUserPreferenceServiceTests
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly CategoryUserPreferenceService _service;
    public CatUserPreferenceServiceTests()
    {
        _httpClientWrapper = A.Fake<IHttpClientWrapper>();
        _service = new CategoryUserPreferenceService(_httpClientWrapper);
    }

    [Fact]
    public async Task CUPService_GetCUPAsync_ReturnsCUP()
    {
        #region Arrange
        string key = "ABC";
        string uname = "dude";
        var cup = new CategoryUserPreference()
        {
            CategoryKey = key,
            Username=uname,
            Value=1
        };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(cup)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetCategoryUserPreferenceAsync(key,uname);

        // Assert
        result.Should().BeEquivalentTo(cup);
    }

    [Fact]
    public async Task CUPService_PutCUPAsync_ReturnsTrue()
    {
        #region Arrange
        string key = "ABC";
        string uname = "dude";
        int newVal = -1;
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };
        A.CallTo(() => _httpClientWrapper
            .PutAsync(A<string>.Ignored,A<HttpContent>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.PutCategoryUserPreferenceAsync(key, uname, newVal);

        // Assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task CUPService_PutCUPAsync_IfNotSuccessReturnsFalse()
    {
        #region Arrange
        string key = "ABC";
        string uname = "dude";
        int newVal = -1;
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.BadRequest
        };
        A.CallTo(() => _httpClientWrapper
            .PutAsync(A<string>.Ignored, A<HttpContent>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.PutCategoryUserPreferenceAsync(key, uname, newVal);

        // Assert
        result.Should().Be(false);
    }
}
