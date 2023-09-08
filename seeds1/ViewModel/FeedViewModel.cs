using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;

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
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryViewModel> feedEntryVMCollection = new();

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
    }

    [ObservableProperty]
    bool isRefreshing = false;
    [RelayCommand]
    public async Task Refresh()
    {
        if (!IsRefreshing)
        {
            IsRefreshing = true;
            FeedEntryVMCollection = new();
            await CollectFeedEntriesPaginated();
            IsRefreshing = false;
        }
    }
    [RelayCommand]
    public async Task CollectFeedEntriesPaginated()
    {
        // This fct. is called in OnNavigatedTo() and from the view.
        int currentCount;

        if (FeedEntryVMCollection == null) { currentCount = 0; }
        else { currentCount = FeedEntryVMCollection.Count; }

        int currentPages = (int)Math.Ceiling((decimal)currentCount / _feedEntryPageSize);

        List<UserFeedentry> feedEntries = new();
        try
        {
            feedEntries = await feedEntriesService.GetUserFeedentriesPaginatedAsync(
                currentPages + 1, _feedEntryPageSize);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error On Collecting FeedEntries", ex.Message, "Ok");
            return;
        }

        if (feedEntries.Count == 0) { return; }
#if WINDOWS
            feedEntries.Reverse();
#endif
        List<FeedEntryViewModel> feedEntryVMs = new();
        foreach (var fe in feedEntries)
        {
            var vm = feedEntryVmFactory.Create();
            vm.FeedEntry = fe;
            feedEntryVMs.Add(vm);
        }
        FeedEntryVMCollection.AddRange(feedEntryVMs);
    }

    /* update all feed entries that have the same categoryKey
     * as the feed entry where the button was clicked.
     * Then update the DB with the new preference.
     */
    [RelayCommand]
    public async Task ChangeTopicPreference(MauiPreference mauiPref)
    {
        int newPref = prefService.StepPreference(mauiPref.Preference);

        // update feed entries
        for (int i = 0; i < FeedEntryVMCollection.Count; i++)
        {
            // loop over topics
            for (int j = 0; j < FeedEntryVMCollection[i].FeedEntry.MauiPreferences.Count; j++)
            {
                if (FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Topic.Id == mauiPref.Topic.Id)
                {
                    FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Preference = newPref;
                }
            }
        }

        // update DB
        await glob.GlobChangePreferenceAsync(mauiPref.Topic.Id, newPref);
    }
}
