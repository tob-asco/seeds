using MvvmHelpers;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace seeds1.Services;

public class GlobalService : IGlobalService{
    private readonly IStaticService stat;
    private readonly IUserPreferenceService userPrefService;
    private readonly IUserIdeaInteractionService uiiService;


    public UserDto CurrentUser { get; set; }
    private Dictionary<Guid, UserPreference> CurrentUserPreferences { get; set; } = new();
    private Dictionary<int, UserIdeaInteraction> CurrentUserIdeaInteractions { get; set; } = new();
    // public, but not in IGlobalService
    public Dictionary<string, ObservableCollection<FamilyOrPreference>> FopListDict { get; set; } = new();
    public List<ObservableCollection<FamilyOrPreference>> FopListList => FopListDict.Values.ToList();
    public bool PreferencesLoaded { get; set; } = false; // public, but not in IGlobalService
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

            // convert
            CurrentUserPreferences = userPreferencesList.ToDictionary(up => up.ItemId);

            #region populate FOPs
            // first the families
            List<FamilyOrPreference> fopList = stat.GetFamilies().Values.Select(f =>
                new FamilyOrPreference()
                {
                    CategoryKey = f.CategoryKey,
                    CategoryName = stat.GetCategories()[f.CategoryKey].Name,
                    IsFamily = true,
                    Family = f,
                }).ToList();
            // second all buttoned tags
            fopList.AddRange(
                userButtonedTagsList.Select(t => new FamilyOrPreference()
                {
                    CategoryKey = t.CategoryKey,
                    CategoryName = stat.GetCategories()[t.CategoryKey].Name,
                    IsFamily = false,
                    Preference = new()
                    {
                        Tag = t,
                        Preference = CurrentUserPreferences.ContainsKey(t.Id) ?
                            CurrentUserPreferences[t.Id].Value : 0,
                    },
                }));
            // now store this list in a grouped dictionary
            FopListDict = fopList
                .GroupBy(fop => fop.CategoryKey)
                .ToDictionary(
                    group => group.Key,
                    group => new ObservableCollection<FamilyOrPreference>(group));
            #endregion

            // inform
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
    public async Task<bool> GlobChangePreferenceAsync(Guid itemId, int newValue)
    {
        bool tagAlreadyButtoned = false;
        try
        {
            // update DB
            await userPrefService.UpsertUserPreferenceAsync(CurrentUser.Username, itemId, newValue);

            #region update FOPs
            if (stat.GetTags().ContainsKey(itemId) &&
                FopListDict.ContainsKey(stat.GetTags()[itemId].CategoryKey))
            {
                if (stat.GetTags()[itemId].FamilyId != null && // Tag is in some family
                    FopListDict[stat.GetTags()[itemId].CategoryKey].FirstOrDefault(fop =>
                        fop.IsFamily == false &&
                        fop.Preference.Tag.Id == itemId) == null) // .. and not yet in FOPs
                {
                    tagAlreadyButtoned = false;
                    FamilyOrPreference newFop = new()
                    {
                        CategoryKey = stat.GetTags()[itemId].CategoryKey,
                        IsFamily = false,
                        Preference = new CatagPreference()
                        { Tag = stat.GetTags()[itemId], Preference = newValue },
                    };
                    FopListDict[stat.GetTags()[itemId].CategoryKey].Add(newFop);
                }
                else // Tag's already buttoned
                {
                    tagAlreadyButtoned = true;
                    FopListDict[stat.GetTags()[itemId].CategoryKey].First(fop =>
                        !fop.IsFamily && fop.Preference.Tag.Id == itemId).Preference.Preference = newValue;
                }
            }
            #endregion

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
            return tagAlreadyButtoned;
        }
        catch (Exception ex)
        { await Shell.Current.DisplayAlert("Error", ex.Message, "Ok"); return false; }
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

    public void Dispose()
    {
        CurrentUser = null!;

        CurrentUserPreferences = new();
        PreferencesLoaded = false;

        CurrentUserIdeaInteractions = new();
        IdeaInteractionsLoaded = false;

        FopListDict = new();
    }
}
