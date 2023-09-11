using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : MyBaseViewModel
{
    private readonly IGlobalService glob;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IUserPreferenceService prefService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryViewModel> feedentryVMs;
    public FeedViewModel(
        IStaticService stat,
        IGlobalService glob,
        IGenericFactory<FeedEntryViewModel> feedEntryVmFactory,
        IUserPreferenceService prefService)
        : base(stat, glob)
    {
        this.glob = glob;
        this.feedEntryVmFactory = feedEntryVmFactory;
        this.prefService = prefService;

        glob.PropertyChanged += OnGlobPropertyChanged;
        FeedentryVMs = glob.FeedentryVMs;
    }

    private void OnGlobPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(glob.FeedentryVMs))
        {
            if (FeedentryVMs != glob.FeedentryVMs)
            {
                FeedentryVMs = glob.FeedentryVMs;
            }
        }
    }

    [RelayCommand]
    public async Task ChangeTopicPreference(MauiPreference mauiPref)
    {
        int newPref = prefService.StepPreference(mauiPref.Preference);
        // change Preference without raising PCE for the Feedentries
        await glob.GlobChangePreferenceAsync(
            mauiPref.Topic.Id, newPref, true, false);

        //for (int i = 0; i < FeedentryVMs.Count; i++)
        //{
        //    // loop over topics
        //    for (int j = 0; j < FeedentryVMs[i].FeedEntry.MauiPreferences.Count; j++)
        //    {
        //        if (FeedentryVMs[i].FeedEntry.MauiPreferences[j].Topic.Id == mauiPref.Topic.Id)
        //        {
        //            FeedentryVMs[i].FeedEntry.MauiPreferences[j].Preference = newPref;
        //        }
        //    }
        //}
    }

    [RelayCommand]
    public async Task MoreFeedentries()
    {
        await glob.MoreFeedentriesAsync();
    }

    [RelayCommand]
    public void Refresh()
    {
        IsBusy = true;
        FeedentryVMs = glob.FeedentryVMs;
        OnPropertyChanged(nameof(FeedentryVMs));
        IsBusy = false;
    }
}
