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

    public async Task<List<TopicFromDb>> GetButtonedTopicsOfUserAsync(string username = "")
    {
        string url = baseUri + $"buttonedTopics";
        if (username != "") { url += $"?username={username}"; }
        return await baseService.GetDalModelAsync<List<TopicFromDb>>(url)
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
    public int StepPreference(int oldPreference)
    {
        if (oldPreference == 0) { return 1; }
        else if (oldPreference == 1) { return -1; }
        else { return 0; }
    }
}
