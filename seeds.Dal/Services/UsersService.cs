using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using System.Web;

namespace seeds.Dal.Services;

public class UsersService : IUsersService
{
    private readonly IDalBaseService _baseService;
    public UsersService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<List<UserDto>> GetUsersAsync()
    {
        string url = "api/Users";
        return await _baseService.GetDalModelAsync<List<UserDto>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }
    public async Task<UserDto?> GetUserByUsernameAsync(string username)
    {
        string url = $"api/Users/{HttpUtility.UrlEncode(username)}";
        return await _baseService.GetDalModelAsync<UserDto>(url);
    }
}
