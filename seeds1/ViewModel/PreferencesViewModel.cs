using MvvmHelpers;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.ViewModel;

//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class PreferencesViewModel : MvvmHelpers.BaseViewModel
{
    private readonly ICategoryPreferencesService catPrefService;
    private readonly ICategoryUserPreferenceService cupService;
    [ObservableProperty]
    ObservableRangeCollection<CatPreference> preferences = new();
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
            Preferences = new();
            Preferences.AddRange(catPrefs);
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
        int index = Preferences.IndexOf(Preferences.FirstOrDefault(cp =>
            cp.Key == categoryKey));
        if (index == -1)
        {
            await Shell.Current.DisplayAlert("Error", "Key not found.", "Ok");
            return;
        }

        Preferences[index].Value = catPrefService.StepCatPreference(
            Preferences[index].Value);

        // update DB
        if (await cupService.PutCategoryUserPreferenceAsync(
            categoryKey,
            CurrentUser.Username,
            Preferences[index].Value) == false)
        {
            await Shell.Current.DisplayAlert("Put Error", "The DB is not updated. Please refresh.", "Ok");
        }
    }
}
