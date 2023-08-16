using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;

namespace seeds1.Interfaces;

public interface IGlobalService
{
    public UserDto CurrentUser { get; set; }

    /// <summary>
    /// Gets all UserPrefernces of the CurrentUser
    /// </summary>
    /// <returns>A dictionary with the key given by the ItemId</returns>
    public Task<Dictionary<Guid, UserPreference>> GetPreferencesAsync();
}
