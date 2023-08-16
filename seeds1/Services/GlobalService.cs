using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;

namespace seeds1.Services;

public class GlobalService : IGlobalService
{
    private readonly IUserPreferenceService userPrefService;
    private readonly IUserIdeaInteractionService uiiService;

    public UserDto CurrentUser { get; set; }
    private Dictionary<Guid, UserPreference> CurrentUserPreferences { get; set; }
    private bool PreferencesPopulated { get; set; } = false;
    public GlobalService(
        IUserPreferenceService userPrefService,
        IUserIdeaInteractionService uiiService)
    {
        this.userPrefService = userPrefService;
        this.uiiService = uiiService;
    }

    public async Task<Dictionary<Guid, UserPreference>> GetPreferencesAsync()
    {
        if (!PreferencesPopulated)
        {
            var userPreferencesList = await userPrefService
                .GetPreferencesOfUserAsync(CurrentUser.Username);
            PreferencesPopulated = true;
            return userPreferencesList.ToDictionary(up => up.ItemId);
        }
        return CurrentUserPreferences;
    }
}
