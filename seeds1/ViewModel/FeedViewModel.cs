using MvvmHelpers;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : BasisViewModel
{
    private static readonly int _maxFeedEntryPageSize = 5;
    private readonly IFeedEntriesService feedEntriesService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly ICatPreferencesService catPrefService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryVM> feedEntryVMCollection = new();

    public FeedViewModel(
        IGlobalVmService globalService,
        IFeedEntriesService feedEntriesService,
        ICategoryUserPreferenceService cupService,
        ICatPreferencesService catPrefService)
        : base(globalService)
    {
        this.feedEntriesService = feedEntriesService;
        this.cupService = cupService;
        this.catPrefService = catPrefService;
    }

    [RelayCommand]
    public async Task CollectFeedEntriesPaginated()
    {
        // This fct. is called in OnNavigatedTo() and from the view.
        int currentCount;

        if (FeedEntryVMCollection == null) { currentCount = 0; }
        else { currentCount = FeedEntryVMCollection.Count; }

        int currentPages = (int)Math.Ceiling((decimal)currentCount / _maxFeedEntryPageSize);
        try
        {
            var feedEntries = await feedEntriesService.GetFeedEntriesPaginated(
                currentPages + 1, _maxFeedEntryPageSize);
            if (feedEntries == null || feedEntries.Count == 0) { return; }
#if WINDOWS
            feedEntries.Reverse();
#endif
            List<FeedEntryVM> feedEntryVMs = new();
            foreach (var fe in feedEntries)
            {
                var vm = Application.Current.Handler.MauiContext.Services.GetService<FeedEntryVM>();
                vm.FeedEntry = fe;
                feedEntryVMs.Add(vm);
            }
            FeedEntryVMCollection.AddRange(feedEntryVMs);
        }
        catch (Exception ex) 
        {
            await Shell.Current.DisplayAlert("Error On Collecting FeedEntries", ex.Message, "Ok");
            return;
        }
    }

    /* update all feed entries that have the same categoryKey
     * as the feed entry where the button was clicked.
     * Then update the DB with the new preference.
     */
    [RelayCommand]
    public async Task ChangeCategoryPreference(string categoryKey)
    {
        // update feed entries
        int? newCatPreference = null;
        for (int i = 0; i < FeedEntryVMCollection.Count; i++)
        {
            if (FeedEntryVMCollection[i].FeedEntry.Idea.CategoryKey == categoryKey)
            {
                FeedEntryVMCollection[i].FeedEntry.CategoryPreference = catPrefService
                    .StepCatPreference(FeedEntryVMCollection[i].FeedEntry.CategoryPreference);

                // for the DB
                newCatPreference ??= FeedEntryVMCollection[i].FeedEntry.CategoryPreference;
            }
        }

        // update DB
        if (newCatPreference != null)
        {
            if (await cupService.PutCategoryUserPreferenceAsync(
                categoryKey,
                CurrentUser.Username,
                (int)newCatPreference) == false)
            {
                await Shell.Current.DisplayAlert("Put Error", "The DB is not updated. Please refresh.", "Ok");
            }
        }
    }

    public async Task LoadCatPreferencesFromDbAsync()
    {
        var catPrefs = await catPrefService.GetCatPreferencesAsync();
        Dictionary<string, int> catPrefsDict = new();
        foreach (var catPref in catPrefs)
        {
            catPrefsDict.Add(catPref.Key, catPref.Value);
        }

        for (int i = 0; i < FeedEntryVMCollection.Count; i++)
        {
            FeedEntryVMCollection[i].FeedEntry
                .CategoryPreference = catPrefsDict[FeedEntryVMCollection[i]
                .FeedEntry.Idea.CategoryKey];
        }
    }
}
