using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class GlobalService : IGlobalService
{
    private readonly IStaticService stat;
    private readonly IUserPreferenceService userPrefService;
    private readonly IUserIdeaInteractionService uiiService;

    public UserDto CurrentUser { get; set; }
    private Dictionary<Guid, UserPreference> CurrentUserPreferences { get; set; }
    private Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { get; set; }
    public List<FamilyOrPreference> FamilyOrPreferences { get; set; }
    private bool PreferencesLoaded { get; set; } = false;
    private bool IdeaInteractionsLoaded { get; set; } = false;
    public GlobalService(
        IStaticService stat,
        IUserPreferenceService userPrefService,
        IUserIdeaInteractionService uiiService)
    {
        this.stat = stat;
        this.userPrefService = userPrefService;
        this.uiiService = uiiService;
    }

    public async Task LoadPreferencesAsync()
    {
        if (!PreferencesLoaded)
        {
            // retrieve
            var userPreferencesList = await userPrefService
                .GetPreferencesOfUserAsync(CurrentUser.Username);
            var userButtonedTagsList = await userPrefService
                .GetButtonedTagsOfUserAsync(CurrentUser.Username);

            // convert and inform
            CurrentUserPreferences = userPreferencesList
                .ToDictionary(up => up.ItemId);
            PreferencesLoaded = true;

            // add Families and Preferences to FamilyOrPreferences
            FamilyOrPreferences.AddRange(
                stat.GetFamilies().Values.Select(f => new FamilyOrPreference()
                {
                    CategoryKey = f.CategoryKey,
                    IsFamily = true,
                    Family = f,
                }));
            FamilyOrPreferences.AddRange(
                userButtonedTagsList.Select(t => new FamilyOrPreference()
                {
                    CategoryKey = t.CategoryKey,
                    IsFamily = false,
                    Preference = new()
                    {
                        Tag = t,
                        Preference = CurrentUserPreferences.ContainsKey(t.Id) ?
                            CurrentUserPreferences[t.Id].Value : 0,
                    },
                }));
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
            // update DB
            await userPrefService.UpsertUserPreferenceAsync(CurrentUser.Username, itemId, newValue);

            // possibly update the FamilyOrPreferences (useful only when Tag is in a Family)
            if (stat.GetTags().ContainsKey(itemId) &&
                stat.GetTags()[itemId].FamilyId != null &&
                FamilyOrPreferences.Find(fop =>
                    fop.IsFamily == false && fop.Preference.Tag.Id == itemId) == null)
            {
                FamilyOrPreferences.Add(new()
                {
                    CategoryKey = stat.GetTags()[itemId].CategoryKey,
                    IsFamily = false,
                    Preference = new CatagPreference()
                    {
                        Tag = stat.GetTags()[itemId],
                        Preference = newValue,
                    },
                }
                );
            }

            // update the member
            if (CurrentUserPreferences.ContainsKey(itemId))
            { CurrentUserPreferences[itemId].Value = newValue; }
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
        { await Shell.Current.DisplayAlert("Error", ex.Message, "Ok"); }
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
