using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;

namespace seeds.Dal.Services;

public class UsersService : IUsersService
{
    private readonly IDalBaseService _baseService;
    public UsersService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<List<UserFromDb>> GetUsersAsync()
    {
        string url = "api/Users";
        return await _baseService.GetDalModelAsync<List<UserFromDb>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }
    public async Task<UserFromDb?> GetUserByUsernameAsync(string username)
    {
        string url = $"api/Users/{username}";
        return await _baseService.GetDalModelAsync<UserFromDb>(url);
    }
}
