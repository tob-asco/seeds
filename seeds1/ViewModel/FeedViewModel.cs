using MvvmHelpers;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;
using seeds1.Services;
using System.ComponentModel;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
[QueryProperty(nameof(RedrawPage),nameof(RedrawPage))]
public partial class FeedViewModel : BasisViewModel
{
    private static readonly int _maxFeedEntryPageSize = 10;
    private readonly IFeedEntriesService _feedEntriesService;
    private readonly IUserIdeaInteractionService _uiiService;
    private readonly IIdeasService _ideasService;
    private readonly ICategoryUserPreferenceService _cupService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntryVM> feedEntryVMCollection;

    public FeedViewModel(
        IFeedEntriesService feedEntryService,
        IUserIdeaInteractionService uiiService,
        IIdeasService ideasService,
        ICategoryUserPreferenceService cupService)
    {
        _feedEntriesService = feedEntryService;
        _uiiService = uiiService;
        _ideasService = ideasService;
        _cupService = cupService;
        FeedEntryVMCollection = new();
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
                FeedEntryVMCollection[i].FeedEntry.CategoryPreference = StepCatPreference(
                    FeedEntryVMCollection[i].FeedEntry.CategoryPreference);

                // for the DB
                newCatPreference ??= FeedEntryVMCollection[i].FeedEntry.CategoryPreference;
            }
        }

        // update DB
        try
        {
            if (newCatPreference == null)
            {
                throw new NullReferenceException(nameof(newCatPreference));
            }
            await _cupService.PutCategoryUserPreferenceAsync(
                categoryKey,
                CurrentUser.Username,
                (int)newCatPreference);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Put Error", ex.Message, "Ignore");
            Console.Write(ex);
        }
    }

    private static int StepCatPreference(int oldPreference)
    {
        if (oldPreference == 0) { return 1; }
        else if (oldPreference == 1) { return -1; }
        else { return 0; }
    }
}
