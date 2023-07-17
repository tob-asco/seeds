using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace seeds.Dal.Tests.Services;

public class CategoryServiceTests
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly CategoryService _service;
    public CategoryServiceTests()
    {
        _httpClientWrapper = A.Fake<IHttpClientWrapper>();
        _service = new CategoryService(_httpClientWrapper);
    }

    [Fact]
    public async Task CatService_GetCatByKeyAsync_ReturnsCategory()
    {
        #region Arrange
        string key = "ABC";
        var cat = new Category()
        {
            Key = key,
            Name = "ABeCe"
        };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(cat)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetCategoryByKeyAsync(key);

        // Assert
        result.Should().BeEquivalentTo(cat);
    }
    [Fact]
    public async Task CatService_GetCatByKeyAsync_IfNotExistReturnsNull()
    {
        #region Arrange
        string key = "ABC";
        var cat = new Category()
        {
            Key = key,
            Name = "ABeCe"
        };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound,
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetCategoryByKeyAsync(key);

        // Assert
        result.Should().BeNull();
    }
}
