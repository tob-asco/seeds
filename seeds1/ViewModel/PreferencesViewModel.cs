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
    private readonly IGlobalService globalService;
    private readonly ICatagPreferencesService prefService;
    private readonly IUserPreferenceService cupService;

    public PreferencesViewModel(
        IStaticService staticService,
        IGlobalService globalService,
        ICatagPreferencesService catPrefService,
        IUserPreferenceService cupService)
        : base(staticService, globalService)
    {
        this.globalService = globalService;
        this.prefService = catPrefService;
        this.cupService = cupService;
    }

    [ObservableProperty]
    ObservableCollection<ObservableRangeCollection<CatagPreference>>
        catagPrefGroups = new();

    [RelayCommand]
    public async Task PopulateListListAsync()
    {
        /* Called also in OnNavigatedTo()
         */
        try
        {
            List<CatagPreference> catagPrefs = prefService.AssembleButtonedUserPreferences();

            var groups = catagPrefs.GroupBy(cp => cp.Tag.CategoryKey);
            foreach (var group in groups)
            {
                ObservableRangeCollection<CatagPreference> tagsOfGroup = new();
                // remove the entries that are only categories
                var tagGroup = group.Where(cp => cp.Tag.Name != null);
                tagsOfGroup.AddRange(tagGroup.ToList());
                CatagPrefGroups.Add(tagsOfGroup);
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Access Error", ex.Message, "Ok");
        }
    }
    [RelayCommand]
    public async Task ChangeTagPreference(CatagPreference pref)
    {
        // find indices
        int groupIndex = CatagPrefGroups.IndexOf(CatagPrefGroups.FirstOrDefault(cpg =>
            cpg.Contains(pref)));
        if (groupIndex == -1)
        {
            await Shell.Current.DisplayAlert("Error", "Group not found.", "Ok");
            return;
        }
        int index = CatagPrefGroups[groupIndex].IndexOf(pref);

        // update DB
        try
        {
            await globalService.GlobChangePreference(
                pref.Tag.Id,
                prefService.StepPreference(CatagPrefGroups[groupIndex][index].Preference));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Put Error",
                ex.Message, "Ok");
            return;
        }

        // update View
        CatagPrefGroups[groupIndex][index].Preference = prefService.StepPreference(
            CatagPrefGroups[groupIndex][index].Preference);
    }
}
