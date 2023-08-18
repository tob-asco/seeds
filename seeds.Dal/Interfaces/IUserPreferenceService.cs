using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IUserPreferenceService
{
    /// <summary>
    /// Corresponding to the new philosophy, we get all preferences
    /// for a certain user in one pack.
    /// </summary>
    /// <param name="username">CurrentUser.Username probably</param>
    /// <returns>A List of UserPreferences</returns>
    public Task<List<UserPreference>> GetPreferencesOfUserAsync(string username = "");
    /// <summary>
    /// Corresponding to the new philosophy, we get all tags that
    /// need buttons in the PreferencesPage in one pack.
    /// Without a username, this function returns all tags that have no family.
    /// Providing a username, the above set reduces by tags with family and user preference.
    /// </summary>
    /// <param name="username">CurrentUser.Username</param>
    /// <returns>A List of TagFromDb's</returns>
    public Task<List<TagFromDb>> GetButtonedTagsOfUserAsync(string username);
    /// <summary>
    /// Posts an upsert request.
    /// </summary>
    /// <returns>Throws exception or nothing.</returns>
    public Task UpsertUserPreferenceAsync(
        string username, Guid itemId, int newValue);
}
