using seeds.Dal.Dto.ToApi;

namespace seeds.Dal.Interfaces;

public interface IUsersService
{
    /* if base returns null, throws.
     */
    public Task<List<UserDtoApi>> GetUsersAsync();
    /* may return null, only base may throw.
     */
    public Task<UserDtoApi?> GetUserByUsernameAsync(string username);
}
