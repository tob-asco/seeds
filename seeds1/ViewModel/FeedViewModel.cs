using MvvmHelpers;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;
using seeds1.Services;
using System.ComponentModel;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : BasisViewModel
{
    private static readonly int _maxFeedEntryPageSize = 10;
    private readonly IFeedEntryService _feedEntryService;
    private readonly ICategoryUserPreferenceService _cupService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntry> feedEntryCollection;
    public FeedViewModel(IFeedEntryService feedEntryService,
        ICategoryUserPreferenceService cupService)
    {
        _feedEntryService = feedEntryService;
        _cupService = cupService;
        FeedEntryCollection = new();
    }

    [RelayCommand]
    public async Task CollectFeedEntriesPaginated()
    {
        // This fct. is called also OnNavigatedTo().

        int currentCount;

        if (FeedEntryCollection == null) { currentCount = 0; }
        else { currentCount = FeedEntryCollection.Count; }

        int currentPages = (int)Math.Ceiling((decimal)currentCount / _maxFeedEntryPageSize);
        try
        {
            _feedEntryService.CurrentUser = CurrentUser;
            var feedEntries = await _feedEntryService.GetFeedEntriesPaginated(
                currentPages + 1, _maxFeedEntryPageSize);
            feedEntries.Reverse();
            FeedEntryCollection.AddRange(feedEntries);
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
        for (int i = 0; i < FeedEntryCollection.Count; i++)
        {
            if (FeedEntryCollection[i].Idea.CategoryKey == categoryKey)
            {
                FeedEntryCollection[i].CategoryPreference = StepCatPreference(
                    FeedEntryCollection[i].CategoryPreference);

                // for the DB
                newCatPreference ??= FeedEntryCollection[i].CategoryPreference;
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
