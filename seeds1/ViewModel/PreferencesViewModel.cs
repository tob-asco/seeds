using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using seeds1.Factories;
using seeds1.Helpers;

namespace seeds1.ViewModel;

//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class PreferencesViewModel : MyBaseViewModel
{
    private readonly IGlobalService glob;
    private readonly IGenericFactory<FamilyPopupViewModel> popupVmFactory;
    private readonly PopupSizeConstants popupSize;
    private readonly ICatagPreferencesService prefService;
    private readonly IUserPreferenceService cupService;

    public PreferencesViewModel(
        IStaticService stat,
        IGlobalService glob,
        IGenericFactory<FamilyPopupViewModel> popupVmFactory,
        PopupSizeConstants popupSize,
        ICatagPreferencesService catPrefService,
        IUserPreferenceService cupService)
        : base(stat, glob)
    {
        this.glob = glob;
        this.popupVmFactory = popupVmFactory;
        this.popupSize = popupSize;
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
    [RelayCommand]
    public async Task PopupFamily(Family fam)
    {
        // find the page to display the popup on
        Page page = Application.Current?.MainPage ?? throw new NullReferenceException();

        // create a ViewModel for the popup
        FamilyPopupViewModel popupVm = popupVmFactory.Create();
        popupVm.Family = fam;

        // display and read result
        await page.ShowPopupAsync(new FamilyPopup(popupVm)
        {
            Size = popupSize.Large
        });
    }
}
