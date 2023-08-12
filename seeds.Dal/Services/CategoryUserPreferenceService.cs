using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class CatagUserPreferenceService : ICatagUserPreferenceService
{
    private readonly IDalBaseService _baseService;
    public CatagUserPreferenceService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<CatagUserPreference> GetCatagUserPreferenceAsync(
        string catKey, string username, string? tagName = null)
    {
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(username)}";
        if (tagName != null) { url += $"?tagName={HttpUtility.UrlEncode(tagName)}"; }
        return await _baseService.GetDalModelAsync<CatagUserPreference>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }

    public async Task<bool> PutCatagUserPreferenceAsync(
        string catKey, string username,
        int newPreference, string? tagName = null)
    {
        string url = $"api/CatagUserPreferences/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(username)}";
        if (tagName != null) { url += $"?tagName={HttpUtility.UrlEncode(tagName)}"; }
        CatagUserPreference newCup = new()
        {
            CategoryKey = catKey,
            Username = username,
            TagName = tagName,
            Value = newPreference
        };
        return await _baseService.PutDalModelAsync<CatagUserPreference>(url, newCup);
    }
}
