using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class UserPreferenceService : IUserPreferenceService
{
    private readonly IDalBaseService baseService;
    private readonly string baseUri = "api/UserPreferences/";
    public UserPreferenceService(IDalBaseService baseService)
    {
        this.baseService = baseService;
    }

    public async Task<List<UserPreference>> GetPreferencesOfUserAsync(string username)
    {
        string url = baseUri + $"{username}";
        return await baseService.GetDalModelAsync<List<UserPreference>>(url)
            ?? throw baseService.ThrowGetNullException(url);
    }

    public async Task<List<TagFromDb>> GetButtonedTagsOfUserAsync(string username = "")
    {
        string url = baseUri + $"buttonedTags";
        if (username != "") { url += $"?username={username}"; }
        return await baseService.GetDalModelAsync<List<TagFromDb>>(url)
            ?? throw baseService.ThrowGetNullException(url);
    }

    public async Task UpsertUserPreferenceAsync(
        string username, Guid itemId, int newValue)
    {
        string url = baseUri + $"upsert";
        UserPreference cup = new()
        {
            ItemId = itemId,
            Username = username,
            Value = newValue
        };
        
        if(!await baseService.PostDalModelBoolReturnAsync(url, cup))
        {
            throw baseService.ThrowPostConflictException(url);
        }
    }
}
