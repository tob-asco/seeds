using seeds.Dal.Dto.FromDb;

namespace seeds.Dal.Interfaces;

public interface IUsersService
{
    /* if base returns null, throws.
     */
    public Task<List<UserFromDb>> GetUsersAsync();
    /* may return null, only base may throw.
     */
    public Task<UserFromDb?> GetUserByUsernameAsync(string username);
}
