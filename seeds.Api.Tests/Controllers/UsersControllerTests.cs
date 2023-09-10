using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using System.Net;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Api.Tests.Controllers;

public class UsersControllerTests : ApiControllerTestsBase
{
    public List<User> Users { get; set; } = new();

    public UsersControllerTests()
        :base(baseUri: "api/Users/")
    {
        PopulatePropertiesAndAddToDb();
        context.SaveChanges();
        // Clear the change tracker, so each test has a fresh _context
        context.ChangeTracker.Clear();
    }

    private void PopulatePropertiesAndAddToDb()
    {
        for (int i = 1; i <= 10; i++)
        {
            Users.Add(new()
            {
                Username = $"t o_b!a${i}?", //unique
                Password = $"!\"£$%^{i}&*()_+",
                Email = "tobi" + i + "@tobi.com", //unique
            });
        }
        if (!context.User.Any()) { context.User.AddRange(Users); }
    }
    [Fact]
    public async Task UsersController_GetUserEndpoint_ReturnsUser()
    {
        //Arrange
        string username = Users[5].Username;
        string url = baseUri + $"{HttpUtility.UrlEncode(username)}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().BeSuccessful();
        var result = await response.Content.ReadFromJsonAsync<UserDto>();
        result.Should().NotBeNull();
        result?.Username.Should().Be(username);
    }
    [Fact]
    public async Task UsersController_GetUserEndpoint_IfNotExistReturnsNotFoundWithMyHeader()
    {
        //Arrange
        string url = baseUri + $"{Guid.NewGuid().ToString()}";

        //Act
        var response = await _httpClient.GetAsync(url);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.NotFound);
        response.Headers.TryGetValues("X-Error-Type", out var vals).Should().BeTrue();
        vals.Should().NotBeNull();
        vals.Should().Contain(s => s == "DbRecordNotFound");
    }
    [Fact]
    public async Task UsersController_PostUserEndpoint_ReturnsSuccessAndUpdatesDb()
    {
        //Arrange
        string username = "tooobi";
        UserDto user = new()
        {
            Username = username,
            Password = "",
            Email = ""
        };
        string url = baseUri;
        HttpContent content = JsonContent.Create(user);

        //Act
        var postResponse = await _httpClient.PostAsync(url, content);

        //Assert
        postResponse.Should().BeSuccessful();
        context.User.Should().Contain(u => u.Username == username);
    }
    /* post the same user twice (id auto-generated) and
     * assert returned Conflict due to (various) uniqueness constraints
     */
    [Fact]
    public async Task UsersController_PostUserEndpoint_ReturnsConflictDuplicate()
    {
        //Arrange
        UserDto user = new()
        {
            Username = "tobi",
            Password = "",
            Email = ""
        };
        string url = baseUri;
        HttpContent content = JsonContent.Create(user);

        //Act
        await _httpClient.PostAsync(url, content);
        var response = await _httpClient.PostAsync(url, content);

        //Assert
        response.Should().HaveStatusCode(HttpStatusCode.Conflict);
    }
}
