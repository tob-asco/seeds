using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IUserPreferenceService
{
    /* If successful returns true,
     * if not-found returns false,
     * else: base throws.
     */
    public Task<bool> PutCatagUserPreferenceAsync(
        string categoryKey, string username, 
        int newPreference, string? tagName = null
        );
}
