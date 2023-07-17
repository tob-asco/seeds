using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;

namespace seeds.Dal.Services;

public class UsersService : DalBaseService, IUsersService
{
    public UsersService(IHttpClientWrapper httpClientWrapper)
        : base(httpClientWrapper) { }
    public async Task<List<User>?> GetUsersAsync()
    {
        string url = "api/Users";
        return await GetDalModelAsync<List<User>>(url);
    }
    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        string url = $"api/Users/{username}";
        return await GetDalModelAsync<User>(url);
    }
}
