using MvvmHelpers;
using seeds.Dal.Interfaces;
using seeds1.Services;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
//[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : BasisViewModel
{
    private static readonly int _maxFeedEntryPageSize = 10;
    private readonly IFeedEntriesService _feedEntriesService;
    private readonly IUserIdeaInteractionService _uiiService;
    private readonly IIdeasService _ideasService;
    private readonly ICategoryUserPreferenceService _cupService;
    private readonly ICatPreferencesService catPrefService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryVM> feedEntryVMCollection = new();

    public FeedViewModel(
        IGlobalVmService globalService,
        IFeedEntriesService feedEntryService,
        IUserIdeaInteractionService uiiService,
        IIdeasService ideasService,
        ICategoryUserPreferenceService cupService,
        ICatPreferencesService catPrefService)
        : base(globalService)
    {
        _feedEntriesService = feedEntryService;
        _uiiService = uiiService;
        _ideasService = ideasService;
        _cupService = cupService;
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
            _feedEntriesService.CurrentUser = CurrentUser;
            var feedEntries = await _feedEntriesService.GetFeedEntriesPaginated(
                currentPages + 1, _maxFeedEntryPageSize);
#if WINDOWS
            feedEntries.Reverse();
#endif
            List<FeedEntryVM> feedEntryVMs = new();
            foreach (var fe in feedEntries)
            {
                fe.Upvotes = await _uiiService.CountVotesAsync(fe.Idea.Id);
                feedEntryVMs.Add(new FeedEntryVM(_uiiService, _ideasService)
                {
                    CurrentUser = CurrentUser,
                    FeedEntry = fe,
                });
            }

            FeedEntryVMCollection.AddRange(feedEntryVMs);
        }
        catch //(Exception ex) 
        {
            //not too bad, possibly just no more ideas
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
            if (await _cupService.PutCategoryUserPreferenceAsync(
                categoryKey,
                CurrentUser.Username,
                (int)newCatPreference) == false)
            {
                await Shell.Current.DisplayAlert("Put Error", "The DB is not updated. Please refresh.", "Ok");
            }
        }
    }
}
