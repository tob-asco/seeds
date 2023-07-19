using seeds.Dal.Dto.ToApi;

namespace seeds.Dal.Interfaces;

public interface IUsersService
{
    public Task<List<UserDtoApi>?> GetUsersAsync();
    public Task<UserDtoApi?> GetUserByUsernameAsync(string username);
}
