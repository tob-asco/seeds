using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class CategoryUserPreferenceService : ICategoryUserPreferenceService
{
    private readonly IDalBaseService _baseService;
    public CategoryUserPreferenceService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<CategoryUserPreference> GetCategoryUserPreferenceAsync(
        string catKey, string username, string? tagName = null)
    {
        string url = $"api/CategoryUserPreferences/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(username)}";
        if (tagName != null) { url += $"?tagName={HttpUtility.UrlEncode(tagName)}"; }
        return await _baseService.GetDalModelAsync<CategoryUserPreference>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }

    public async Task<bool> PutCategoryUserPreferenceAsync(
        string catKey, string username,
        int newPreference, string? tagName = null)
    {
        string url = $"api/CategoryUserPreferences/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(username)}";
        if (tagName != null) { url += $"?tagName={HttpUtility.UrlEncode(tagName)}"; }
        CategoryUserPreference newCup = new()
        {
            CategoryKey = catKey,
            Username = username,
            TagName = tagName,
            Value = newPreference
        };
        return await _baseService.PutDalModelAsync<CategoryUserPreference>(url, newCup);
    }
}
