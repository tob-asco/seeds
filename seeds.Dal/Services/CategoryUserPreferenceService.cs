using seeds.Dal.Interfaces;
using seeds.Dal.Model;

namespace seeds.Dal.Services;

public class CategoryUserPreferenceService : ICategoryUserPreferenceService
{
    private readonly IDalBaseService _baseService;
    public CategoryUserPreferenceService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<CategoryUserPreference> GetCategoryUserPreferenceAsync(
        string categoryKey, string username, string? tagName = null)
    {
        string url = $"api/CategoryUserPreferences/{categoryKey}/{username}";
        if (tagName != null) { url += $"?tagName={tagName}"; }
        return await _baseService.GetDalModelAsync<CategoryUserPreference>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }

    public async Task<bool> PutCategoryUserPreferenceAsync(
        string categoryKey, string username,
        int newPreference, string? tagName = null)
    {
        string url = $"api/CategoryUserPreferences/{categoryKey}/{username}";
        if (tagName != null) { url += $"?tagName={tagName}"; }
        CategoryUserPreference newCup = new()
        {
            CategoryKey = categoryKey,
            Username = username,
            TagName = tagName,
            Value = newPreference
        };
        return await _baseService.PutDalModelAsync<CategoryUserPreference>(url, newCup);
    }
}
