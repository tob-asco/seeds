using seeds.Dal.Dto.FromDb;
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
    public Dictionary<Guid, TagFromDb> CurrentUserButtonedTags { private get; set; }
    private Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { get; set; }
    private bool PreferencesLoaded { get; set; } = false;
    private bool ButtonedTagsLoaded { get; set; } = false;
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
    public async Task GlobChangePreferenceAsync(Guid itemId, int newValue)
    {
        try
        {
            await userPrefService.UpsertUserPreferenceAsync(CurrentUser.Username, itemId, newValue);
            if (CurrentUserPreferences.ContainsKey(itemId))
            {
                CurrentUserPreferences[itemId].Value = newValue;
            }
            else
            {
                CurrentUserPreferences.Add(itemId, new()
                {
                    Username = CurrentUser.Username,
                    ItemId = itemId,
                    Value = newValue,
                });
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Upsert Error", ex.Message, "Ok");
        }
    }
    public async Task LoadButtonedTagsAsync()
    {
        if (!ButtonedTagsLoaded)
        {
            var userButtonedTagsList = await userPrefService
                .GetButtonedTagsOfUserAsync(CurrentUser.Username);
            CurrentUserButtonedTags = userButtonedTagsList
                .ToDictionary(t => t.Id);
            ButtonedTagsLoaded = true;
        }
    }
    public Dictionary<Guid, TagFromDb> GetButtonedTags()
    {
        if (!ButtonedTagsLoaded)
        {
            throw new InvalidOperationException("ButtonedTags not yet loaded.");
        }
        else { return CurrentUserButtonedTags; }
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
    public async Task GlobChangeIdeaInteractionAsync(UserIdeaInteraction newUii)
    {
        try
        {
            await uiiService.PostOrPutUserIdeaInteractionAsync(newUii);
            if (CurrentUserIdeaInteractions.ContainsKey(newUii.IdeaId))
            {
                CurrentUserIdeaInteractions[newUii.IdeaId] = newUii;
            }
            else { CurrentUserIdeaInteractions.Add(newUii.IdeaId, newUii); }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Upsert Error", ex.Message, "Ok");
        }
    }

}
