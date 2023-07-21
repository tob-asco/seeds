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

    [ObservableProperty]
    ObservableRangeCollection<CatPreference> catPreferences = new();
    public PreferencesViewModel(
        IGlobalVmService globalService,
        ICatPreferencesService catPrefService)
        : base(globalService)
    {
        this.catPrefService = catPrefService;
    }

    [RelayCommand]
    public async Task GetCatPreferencesAsync()
    {
        /* Called also in OnNavigatedTo()
         */
        catPrefService.CurrentUser = CurrentUser;
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
}
