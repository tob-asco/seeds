using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class CategoryUserPreferenceService : DalBaseService, ICategoryUserPreferenceService
{
    public CategoryUserPreferenceService(IHttpClientWrapper httpClientWrapper)
        : base(httpClientWrapper) { }
    public async Task<CategoryUserPreference?> GetCategoryUserPreferenceAsync(
        string categoryKey, string username)
    {
        string url = $"api/CategoryUserPreferences/{categoryKey}/{username}";
        return await GetDalModelAsync<CategoryUserPreference>(url);
    }

    // returns true if HttpStatusCode is a successful one
    public async Task<bool> PutCategoryUserPreferenceAsync(string categoryKey, string username, int newPreference)
    {
        string url = $"api/CategoryUserPreferences/{categoryKey}/{username}";
        CategoryUserPreference newCup = new()
        {
            CategoryKey = categoryKey,
            Username = username,
            Value = newPreference
        };
        return await PutDalModelAsync<CategoryUserPreference>(url, newCup);
    }
}
