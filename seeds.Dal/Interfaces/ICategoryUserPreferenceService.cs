using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface ICategoryUserPreferenceService
{
    /* If base returns null, throws.
     */
    public Task<CategoryUserPreference> GetCategoryUserPreferenceAsync(
        string categoryKey, string username, string? tagName = null);
    /* If successful returns true,
     * if not-found returns false,
     * else: base throws.
     */
    public Task<bool> PutCategoryUserPreferenceAsync(
        string categoryKey, string username, 
        int newPreference, string? tagName = null
        );
}
