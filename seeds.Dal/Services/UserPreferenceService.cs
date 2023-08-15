using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IDalBaseService _baseService;
    public UserPreferenceService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<UserPreference> GetCatagUserPreferenceAsync(
        string catKey, string username, string? tagName = null)
    {
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(username)}";
        if (tagName != null) { url += $"?tagName={HttpUtility.UrlEncode(tagName)}"; }
        return await _baseService.GetDalModelAsync<UserPreference>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }

    public async Task<bool> PutCatagUserPreferenceAsync(
        string catKey, string username,
        int newPreference, string? tagName = null)
    {
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(username)}";
        if (tagName != null) { url += $"?tagName={HttpUtility.UrlEncode(tagName)}"; }
        UserPreference newCup = new()
        {
            CategoryKey = catKey,
            Username = username,
            TagName = tagName,
            Value = newPreference
        };
        return await _baseService.PutDalModelAsync<UserPreference>(url, newCup);
    }
}
