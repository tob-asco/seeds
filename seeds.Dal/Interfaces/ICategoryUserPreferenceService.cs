using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface ICategoryUserPreferenceService
{
    public Task<CategoryUserPreference> GetCategoryUserPreferenceAsync(
        string categoryKey,
        string username
        );
    // returns a bool of success
    public Task<bool> PutCategoryUserPreferenceAsync(
        string categoryKey,
        string username,
        int newPreference
        );
}
