using MvvmHelpers;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel :MyBaseViewModel
{
    private static readonly int _feedEntryPageSize = 5;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IUserPreferenceService prefService;
    private readonly IFeedEntriesService feedEntriesService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryViewModel> feedEntryVMCollection = new();

    public FeedViewModel(
        IStaticService staticService,
        IGlobalService globalService,
        IGenericFactory<FeedEntryViewModel> feedEntryVmFactory,
        IUserPreferenceService prefService,
        IFeedEntriesService feedEntriesService)
        : base(staticService, globalService)
    {
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
        if (mauiPref.Topic.Name == null) return;
        // update feed entries
        int? newCatPreference = null;
        for (int i = 0; i < FeedEntryVMCollection.Count; i++)
        {
            // loop over topics
            for (int j = 0; j < FeedEntryVMCollection[i].FeedEntry.MauiPreferences.Count; j++)
            { 
                if (FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Topic.CategoryKey == mauiPref.Topic.CategoryKey
                 && FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Topic.Name == mauiPref.Topic.Name)
                {
                    FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Preference = prefService
                        .StepPreference(FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Preference);

                    // for the DB
                    newCatPreference ??= FeedEntryVMCollection[i].FeedEntry.MauiPreferences[j].Preference;
                }
            }
        }

        // update DB
        if (newCatPreference != null)
        {
            try
            {
                //if (!await cupService.PutUserPreferenceAsync(
                //    mauiPref.CategoryKey,
                //    CurrentUser.Username,
                //    (int)newCatPreference,
                //    topicName: mauiPref.TopicName))
                //{
                //    throw new Exception($"Fatal: Could not Put.");
                //}
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("DB Error",
                    ex.Message, "Ok");
            }
        }
    }
}
