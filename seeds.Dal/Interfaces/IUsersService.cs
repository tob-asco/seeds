using seeds.Dal.Dto.ToAndFromDb;

namespace seeds.Dal.Interfaces;

public interface IUsersService
{
    /* if base returns null, throws.
     */
    public Task<List<UserDto>> GetUsersAsync();
    /* may return null, only base may throw.
     */
    public Task<UserDto?> GetUserByUsernameAsync(string username);
}
