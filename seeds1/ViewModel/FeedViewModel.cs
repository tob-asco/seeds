using CommunityToolkit.Maui.Core.Extensions;
using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;
using System.Collections.ObjectModel;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : MyBaseViewModel
{
    private static readonly int _feedEntryPageSize = 5;
    private readonly IGlobalService glob;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IUserPreferenceService prefService;
    private readonly IFeedEntriesService feedEntriesService;
    ObservableCollection<FeedEntryViewModel> feedentryVMs = new();
    public ObservableCollection<FeedEntryViewModel> FeedentryVMs
    {
        get => feedentryVMs;
        set
        {
            if (feedentryVMs != value)
            {
                feedentryVMs = value;
                OnPropertyChanged(nameof(FeedentryVMs));
            }
        }
    }
    public FeedViewModel(
        IStaticService stat,
        IGlobalService glob,
        IGenericFactory<FeedEntryViewModel> feedEntryVmFactory,
        IUserPreferenceService prefService,
        IFeedEntriesService feedEntriesService)
        : base(stat, glob)
    {
        this.glob = glob;
        this.feedEntryVmFactory = feedEntryVmFactory;
        this.prefService = prefService;
        this.feedEntriesService = feedEntriesService;
        feedentryVMs = glob.FeedentryVMs;
    }

    /// <summary>
    /// Update all feed entries that have the same topic
    /// as the topic clicked.
    /// Then update the DB with the new preference.
    /// </summary>
    /// <param name="mauiPref">The tapped topic as MauiPreference</param>
    [RelayCommand]
    public async Task ChangeTopicPreference(MauiPreference mauiPref)
    {
        int newPref = prefService.StepPreference(mauiPref.Preference);

        // update feed entries
        for (int i = 0; i < FeedentryVMs.Count; i++)
        {
            // loop over topics
            for (int j = 0; j < FeedentryVMs[i].FeedEntry.MauiPreferences.Count; j++)
            {
                if (FeedentryVMs[i].FeedEntry.MauiPreferences[j].Topic.Id == mauiPref.Topic.Id)
                {
                    FeedentryVMs[i].FeedEntry.MauiPreferences[j].Preference = newPref;
                }
            }
        }

        // update DB
        await glob.GlobChangePreferenceAsync(mauiPref.Topic.Id, newPref);
    }

    [RelayCommand]
    public async Task MoreFeedentries()
    {
        await glob.MoreFeedentriesAsync();
    }
}
