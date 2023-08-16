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
    private Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { get; set; }
    private bool IdeaInteractionsPopulated { get; set; } = false;
    public GlobalService(
        IUserPreferenceService userPrefService,
        IUserIdeaInteractionService uiiService)
    {
        this.userPrefService = userPrefService;
        this.uiiService = uiiService;
    }

    /// <summary>
    /// Gets all UserPrefernces of the CurrentUser
    /// </summary>
    /// <returns>A dictionary with the key given by the ItemId</returns>
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

    /// <summary>
    /// Gets all UserIdeaInteractions of the CurrentUser
    /// </summary>
    /// <returns>A dictionary with the key given by the IdeaId</returns>
    public async Task<Dictionary<int, UserIdeaInteraction>> GetIdeaInteractionsAsync()
    {
        if (!IdeaInteractionsPopulated)
        {
            var ideaInteractionsList = await uiiService
                .GetIdeaInteractionsOfUserAsync(CurrentUser.Username);
            IdeaInteractionsPopulated = true;
            return ideaInteractionsList.ToDictionary(uii => uii.IdeaId);
        }
        return CurrentUserIdeaInteractions;
    }
}
