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

public class IdeasServiceTests
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    private readonly IdeasService _service;
    public IdeasServiceTests()
    {
        _httpClientWrapper = A.Fake<IHttpClientWrapper>();
        _service = new IdeasService(_httpClientWrapper);
    }

    [Fact]
    public async void IdeasService_GetIdeasPaginated_ReturnsSameIdeas()
    {
        #region Arrange
        int page = 2; int maxPageSize = 10;
        var users = new List<Idea>()
        {
            new Idea{ Title = "1st Idea" },
            new Idea{ Title = "2nd Idea" },
        };
        var response = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StringContent(JsonSerializer.Serialize(users)),
        };
        A.CallTo(() => _httpClientWrapper.GetAsync(A<string>.Ignored))
            .Returns(response);
        #endregion

        // Act
        var result = await _service.GetIdeasPaginatedAsync(page, maxPageSize);

        // Assert
        result.Should().BeEquivalentTo(users);
    }
}
