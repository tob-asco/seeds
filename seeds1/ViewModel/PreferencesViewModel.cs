using MvvmHelpers;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.ViewModel;

//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class PreferencesViewModel : MyBaseViewModel
{
    private readonly ICategoryPreferencesService catPrefService;
    private readonly ICategoryUserPreferenceService cupService;
    [ObservableProperty]
    ObservableRangeCollection<CatPreference> catPrefs = new();
    public PreferencesViewModel(
        IGlobalService globalService,
        ICategoryPreferencesService catPrefService,
        ICategoryUserPreferenceService cupService)
        : base(globalService)
    {
        this.catPrefService = catPrefService;
        this.cupService = cupService;
    }

    [RelayCommand]
    public async Task GetCatPreferencesAsync()
    {
        /* Called also in OnNavigatedTo()
         */
        try
        {
            var catPrefs = await catPrefService.GetCatPreferencesAsync();
            CatPrefs = new();
            CatPrefs.AddRange(catPrefs);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    [RelayCommand]
    public async Task ChangeCategoryPreference(string categoryKey)
    {
        // update entry
        int index = CatPrefs.IndexOf(CatPrefs.FirstOrDefault(cp =>
            cp.Key == categoryKey));
        if (index == -1)
        {
            await Shell.Current.DisplayAlert("Error", "Key not found.", "Ok");
            return;
        }

        CatPrefs[index].Value = catPrefService.StepCatPreference(
            CatPrefs[index].Value);

        // update DB
        if (await cupService.PutCategoryUserPreferenceAsync(
            categoryKey,
            CurrentUser.Username,
            CatPrefs[index].Value) == false)
        {
            await Shell.Current.DisplayAlert("Put Error", "The DB is not updated. Please refresh.", "Ok");
        }
    }
}
