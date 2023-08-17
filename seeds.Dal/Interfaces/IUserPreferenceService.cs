using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IUserPreferenceService
{
    /// <summary>
    /// Corresponding to the new philosophy we get all preferences
    /// for a certain user, to reduce the number of GET calls.
    /// </summary>
    /// <param name="username">CurrentUser.Username probably</param>
    /// <returns>A List of UserPreferences</returns>
    public Task<List<UserPreference>> GetPreferencesOfUserAsync(string username);
    /// <summary>
    /// Posts an upsert request.
    /// </summary>
    /// <returns>Throws exception or nothing.</returns>
    public Task UpsertUserPreferenceAsync(
        string username, Guid itemId, int newValue);
}
