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
    private readonly IGlobalService glob;
    private readonly ICatagPreferencesService prefService;
    private readonly IUserPreferenceService cupService;

    public PreferencesViewModel(
        IStaticService stat,
        IGlobalService glob,
        ICatagPreferencesService catPrefService,
        IUserPreferenceService cupService)
        : base(stat, glob)
    {
        this.glob = glob;
        this.prefService = catPrefService;
        this.cupService = cupService;
    }

    [ObservableProperty]
    ObservableRangeCollection<ObservableRangeCollection<FamilyOrPreference>>
        fopGroups = new();

    [RelayCommand]
    public async Task PopulateListListAsync()
    {
        /* Called also in OnNavigatedTo()
         */
        try
        {
            var groups = glob.FamilyOrPreferences.GroupBy(fop => fop.CategoryKey);
            FopGroups.AddRange(groups
                .Select(group => new ObservableRangeCollection<FamilyOrPreference>(group)));
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "Ok");
        }
    }
    [RelayCommand]
    public async Task ChangeTagPreference(CatagPreference pref)
    {
        // update DB
        await glob.GlobChangePreferenceAsync(
            pref.Tag.Id, prefService.StepPreference(pref.Preference));

        // update View
        pref.Preference = prefService.StepPreference(
            pref.Preference);
    }
}
