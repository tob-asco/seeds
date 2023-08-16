using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IUserPreferenceService
{
    /// <summary>
    /// Posts an upsert request.
    /// </summary>
    /// <returns>Throws exception or nothing.</returns>
    public Task UpsertUserPreferenceAsync(
        string username, Guid itemId, int newValue);
}
