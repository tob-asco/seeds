using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;

namespace seeds1.ViewModel;

//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class PreferencesViewModel : MyBaseViewModel
{
    private readonly ICatagPreferencesService catPrefService;
    private readonly ICatagUserPreferenceService cupService;
    private readonly ITagFamilyService tagFamService;

    public PreferencesViewModel(
        IGlobalService globalService,
        ICatagPreferencesService catPrefService,
        ICatagUserPreferenceService cupService,
        ITagFamilyService tagFamService)
        : base(globalService)
    {
        this.catPrefService = catPrefService;
        this.cupService = cupService;
        this.tagFamService = tagFamService;
    }

    [ObservableProperty]
    ObservableCollection<ObservableRangeCollection<FamilyOrPreference>>
        famOrPrefsGroups = new();

    [RelayCommand]
    public async Task PopulateListListAsync()
    {
        /* Called also in OnNavigatedTo()
         */
        try
        {
            List<CatagPreference> catagPrefs = 
                await catPrefService.GetCatagPreferencesAsync();

            var groups = catagPrefs.GroupBy(cp => cp.CategoryKey);
            foreach (var group in groups)
            {
                ObservableRangeCollection<FamilyOrPreference> famOrPrefsGroup = new();
                // remove the entries that are only categories
                var tagGroup = group.Where(cp => cp.TagName != null);
                famOrPrefsGroup.AddRange(
                    tagFamService.ConvertToFamilyOrPreferences(tagGroup.ToList()));
                FamOrPrefsGroups.Add(famOrPrefsGroup);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Access Error", ex.Message, "Ok");
        }
    }

    [RelayCommand]
    public async Task ChangeTagPreference(FamilyOrPreference famOrPref)
    {
        // find indices
        int groupIndex = FamOrPrefsGroups.IndexOf(
            FamOrPrefsGroups.FirstOrDefault(fopg => fopg.Contains(famOrPref)));
        if (groupIndex == -1)
        {
            await Shell.Current.DisplayAlert("Error", "Group not found.", "Ok");
            return;
        }
        int index = FamOrPrefsGroups[groupIndex].IndexOf(famOrPref);

        // update DB
        try
        {
            if (FamOrPrefsGroups[groupIndex][index].IsFamily)
            {
                throw new Exception("This is a family, not a single tag.");
            }
            if (!await cupService.PutCatagUserPreferenceAsync(
                famOrPref.CatagPreference.CategoryKey,
                CurrentUser.Username,
                catPrefService.StepPreference(
                    FamOrPrefsGroups[groupIndex][index].CatagPreference.Preference),
                tagName: famOrPref.CatagPreference.TagName))
            {
                throw new Exception($"Fatal: PUT failed.");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error",
                ex.Message, "Ok");
            return;
        }
        
        // update View
        FamOrPrefsGroups[groupIndex][index].CatagPreference.Preference
            = catPrefService.StepPreference(
            FamOrPrefsGroups[groupIndex][index].CatagPreference.Preference);
    }
}
