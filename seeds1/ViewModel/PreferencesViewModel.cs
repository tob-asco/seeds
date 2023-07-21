using MvvmHelpers;
using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.ViewModel;

//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class PreferencesViewModel : BasisViewModel
{
    private readonly ICatPreferencesService catPrefService;
    private readonly ICategoryUserPreferenceService cupService;
    [ObservableProperty]
    ObservableRangeCollection<CatPreference> catPreferences = new();
    public PreferencesViewModel(
        IGlobalVmService globalService,
        ICatPreferencesService catPrefService,
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
            CatPreferences = new();
            CatPreferences.AddRange(catPrefs);
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
        int index = CatPreferences.IndexOf(CatPreferences.FirstOrDefault(cp =>
            cp.Key == categoryKey));
        if (index == -1)
        {
            await Shell.Current.DisplayAlert("Error", "Key not found.", "Ok");
            return;
        }

        CatPreferences[index].Value = catPrefService.StepCatPreference(
         CatPreferences[index].Value);

        // update DB
        if (await cupService.PutCategoryUserPreferenceAsync(
            categoryKey,
            CurrentUser.Username,
            CatPreferences[index].Value) == false)
        {
            await Shell.Current.DisplayAlert("Put Error", "The DB is not updated. Please refresh.", "Ok");
        }
    }
}
