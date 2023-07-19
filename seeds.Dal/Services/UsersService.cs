using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;

namespace seeds.Dal.Services;

public class UsersService : IUsersService
{
    private readonly IDalBaseService _baseService;
    public UsersService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<List<UserDtoApi>?> GetUsersAsync()
    {
        string url = "api/Users";
        return await _baseService.GetDalModelAsync<List<UserDtoApi>>(url);
    }
    public async Task<UserDtoApi?> GetUserByUsernameAsync(string username)
    {
        string url = $"api/Users/{username}";
        return await _baseService.GetDalModelAsync<UserDtoApi>(url);
    }
}
