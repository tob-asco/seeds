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
        string categoryKey, string username)
    {
        string url = $"api/CategoryUserPreferences/{categoryKey}/{username}";
        return await _baseService.GetDalModelAsync<CategoryUserPreference>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }

    public async Task<bool> PutCategoryUserPreferenceAsync(
        string categoryKey,
        string username,
        int newPreference)
    {
        string url = $"api/CategoryUserPreferences/{categoryKey}/{username}";
        CategoryUserPreference newCup = new()
        {
            CategoryKey = categoryKey,
            Username = username,
            Value = newPreference
        };
        return await _baseService.PutDalModelAsync<CategoryUserPreference>(url, newCup);
    }
}
