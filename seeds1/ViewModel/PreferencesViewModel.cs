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

    public PreferencesViewModel(
        IGlobalService globalService,
        ICatagPreferencesService catPrefService,
        ICatagUserPreferenceService cupService)
        : base(globalService)
    {
        this.catPrefService = catPrefService;
        this.cupService = cupService;
    }

    [ObservableProperty]
    ObservableRangeCollection<CatagPreference> catPrefs = new();
    [ObservableProperty]
    ObservableCollection<ObservableRangeCollection<CatagPreference>>
        catagPrefGroups = new();
    [ObservableProperty]
    bool isRefreshing = false;

    [RelayCommand]
    public async Task Refresh()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;
            await PopulateListListAsync();
            IsRefreshing = false;
        }
    }
    [RelayCommand]
    public async Task PopulateListListAsync()
    {
        /* Called also in OnNavigatedTo()
         */
        try
        {
            //CatPrefs = new();
            //var catPrefs = await catPrefService.GetCatPreferencesAsync();
            //CatPrefs.AddRange(catPrefs);
            List<CatagPreference> catagPrefs = 
                await catPrefService.GetCatagPreferencesAsync();

            var groups = catagPrefs.GroupBy(cp => cp.CategoryKey);
            foreach (var group in groups)
            {
                ObservableRangeCollection<CatagPreference> catagsOfGroup = new();
                catagsOfGroup.AddRange(group.ToList());
                CatagPrefGroups.Add(catagsOfGroup);
            }
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
            cp.CategoryKey == categoryKey));
        if (index == -1)
        {
            await Shell.Current.DisplayAlert("Error", "Key not found.", "Ok");
            return;
        }

        CatPrefs[index].Preference = catPrefService.StepPreference(
            CatPrefs[index].Preference);

        // update DB
        try
        {
            if (await cupService.PutCatagUserPreferenceAsync(
                categoryKey,
                CurrentUser.Username,
                CatPrefs[index].Preference) == false)
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
