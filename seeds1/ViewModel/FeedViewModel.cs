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
    private readonly IFeedEntriesService feedEntriesService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly ICatagPreferencesService catPrefService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryViewModel> feedEntryVMCollection = new();

    public FeedViewModel(
        IGlobalService globalService,
        IGenericFactory<FeedEntryViewModel> feedEntryVmFactory,
        IFeedEntriesService feedEntriesService,
        ICategoryUserPreferenceService cupService,
        ICatagPreferencesService catPrefService)
        : base(globalService)
    {
        this.feedEntryVmFactory = feedEntryVmFactory;
        this.feedEntriesService = feedEntriesService;
        this.cupService = cupService;
        this.catPrefService = catPrefService;
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

        List<FeedEntry> feedEntries = new();
        try
        {
            feedEntries = await feedEntriesService.GetFeedEntriesPaginatedAsync(
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
            try
            {
                if (await cupService.PutCategoryUserPreferenceAsync(
                    categoryKey,
                    CurrentUser.Username,
                    (int)newCatPreference) == false)
                {
                    throw new Exception($"The user preference for category {categoryKey}" +
                        $" could not be Put. Please refresh.");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("DB Error",
                    ex.Message, "Ok");
            }
        }
    }

    public async Task LoadCatPreferencesFromDbAsync()
    {
        //List<CatagPreference> catagPrefs = new();
        //try
        //{
        //    catagPrefs = await catPrefService.GetCatagPreferencesAsync();
        //}
        //catch (Exception ex)
        //{
        //    await Shell.Current.DisplayAlert("DB Error",
        //        ex.Message, "Ok");
        //}
        //Dictionary<string, int> catagPrefsDict = new();
        //foreach (var catagPref in catagPrefs)
        //{
        //    catagPrefsDict.Add(catagPref.CategoryKey, catagPref.Preference);
        //}

        //for (int i = 0; i < FeedEntryVMCollection.Count; i++)
        //{
        //    FeedEntryVMCollection[i].FeedEntry
        //        .CategoryPreference = catagPrefsDict[FeedEntryVMCollection[i]
        //        .FeedEntry.Idea.CategoryKey];
        //}
    }
}
