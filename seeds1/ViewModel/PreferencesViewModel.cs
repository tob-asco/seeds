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
            CatPrefs = new();
            var catPrefs = await catPrefService.GetCatPreferencesAsync();
            CatPrefs.AddRange(catPrefs);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Access Error", ex.Message, "Ok");
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
        try
        {
            if (await cupService.PutCategoryUserPreferenceAsync(
                categoryKey,
                CurrentUser.Username,
                CatPrefs[index].Value) == false)
            {
                throw new Exception($"The user preference for category {categoryKey}" +
                    $" was not found, so could not be Put. Although it should exist.");
            }
        }
        catch (Exception ex) 
        {
            await Shell.Current.DisplayAlert("Put Error",
                ex.Message, "Ok");
        }
    }
}
