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
    public Dictionary<Guid, UserPreference> CurrentUserPreferences { private get; set; }
    private bool PreferencesLoaded { get; set; } = false;
    public Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { private get; set; }
    private bool IdeaInteractionsLoaded { get; set; } = false;
    public GlobalService(
        IUserPreferenceService userPrefService,
        IUserIdeaInteractionService uiiService)
    {
        this.userPrefService = userPrefService;
        this.uiiService = uiiService;
    }

    public async Task LoadPreferencesAsync()
    {
        if (!PreferencesLoaded)
        {
            var userPreferencesList = await userPrefService
                .GetPreferencesOfUserAsync(CurrentUser.Username);
            CurrentUserPreferences = userPreferencesList
                .ToDictionary(up => up.ItemId);
            PreferencesLoaded = true;
        }
    }
    public Dictionary<Guid, UserPreference> GetPreferences()
    {
        if (!PreferencesLoaded)
        {
            throw new InvalidOperationException("Preferences not yet loaded.");
        }
        else { return CurrentUserPreferences; }
    }

    public async Task LoadIdeaInteractionsAsync()
    {
        if (!IdeaInteractionsLoaded)
        {
            var ideaInteractionsList = await uiiService
                .GetIdeaInteractionsOfUserAsync(CurrentUser.Username);
            CurrentUserIdeaInteractions = ideaInteractionsList
                .ToDictionary(uii => uii.IdeaId);
            IdeaInteractionsLoaded = true;
        }
    }
    public Dictionary<int, UserIdeaInteraction> GetIdeaInteractions()
    {
        if (!PreferencesLoaded)
        {
            throw new InvalidOperationException("IdeaInteractions not yet loaded.");
        }
        else { return CurrentUserIdeaInteractions; }
    }
}
