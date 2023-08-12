using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface ICatagUserPreferenceService
{
    /* If base returns null, throws.
     */
    public Task<CatagUserPreference> GetCatagUserPreferenceAsync(
        string categoryKey, string username, string? tagName = null);
    /* If successful returns true,
     * if not-found returns false,
     * else: base throws.
     */
    public Task<bool> PutCatagUserPreferenceAsync(
        string categoryKey, string username, 
        int newPreference, string? tagName = null
        );
}
