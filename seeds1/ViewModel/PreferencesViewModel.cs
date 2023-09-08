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
using seeds.Dal.Dto.FromDb;
using CommunityToolkit.Maui.Alerts;
using System.ComponentModel;
using CommunityToolkit.Maui.Core;

namespace seeds1.ViewModel;

//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class PreferencesViewModel : MyBaseViewModel
{
    public readonly IGlobalService glob;
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

        fopListList = glob.FopListList;
    }

    private List<ObservableCollection<FamilyOrPreference>> fopListList = new();
    public List<ObservableCollection<FamilyOrPreference>> FopListList
    {
        get { return glob.FopListList; }
        set
        {
            if (fopListList != value)
            {
                fopListList = value;
                OnPropertyChanged(nameof(FopListList));
            }
        }
    }

    [RelayCommand]
    public async Task ChangeTagPreference(CatagPreference pref)
    {
        // update DB
        await glob.GlobChangePreferenceAsync(
            pref.Tag.Id, prefService.StepPreference(pref.Preference));

        // update View
        //pref.Preference = prefService.StepPreference(
        //    pref.Preference);
    }
    [RelayCommand]
    public async Task PopupFamily(FamilyFromDb fam)
    {
        // find the page to display the popup on
        Page page = Microsoft.Maui.Controls.Application.Current?.MainPage
            ?? throw new NullReferenceException();

        // create a ViewModel for the popup
        FamilyPopupViewModel popupVm = popupVmFactory.Create();
        popupVm.WholeFamily = fam;

        // display and read result
        Size size = popupSize.Tiny;
        if (fam.Tags.Count > 18)
        { size = popupSize.Large; }
        else if (fam.Tags.Count > 12)
        { size = popupSize.Medium; }
        if (fam.Tags.Count > 6)
        { size = popupSize.Small; }

        if (await page.ShowPopupAsync(new FamilyPopup(popupVm)
        {
            Size = size
        }) is TagFromDb chosenTag && chosenTag.CategoryKey == fam.CategoryKey)
        {
            // update DB and FopListList
            if (await glob.GlobChangePreferenceAsync(chosenTag.Id, 1))
            {
                Toast.Make("It's already in your list.");
            }
        }
    }
}
