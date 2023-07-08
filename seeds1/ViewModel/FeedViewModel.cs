using MvvmHelpers;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.ViewModel;

//    ...     ( property here ... , queryId    ...   ))]
[QueryProperty(nameof(CurrentUser),nameof(CurrentUser))] //available AFTER ctor, ...
public partial class FeedViewModel : BasisViewModel
{    
    private static readonly int _maxFeedEntryPageSize = 10;
    private readonly IFeedEntryService _feedEntryService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntry> feedEntryCollection;
    public FeedViewModel(IFeedEntryService feedEntryService)
    {
        _feedEntryService = feedEntryService;
        FeedEntryCollection = new();
    }

    [RelayCommand]
    public async Task CollectFeedEntriesPaginated()
    {
        int currentCount;

        if (FeedEntryCollection == null) { currentCount = 0; }
        else { currentCount = FeedEntryCollection.Count; }

        int currentPages = (int)Math.Ceiling((decimal)currentCount / _maxFeedEntryPageSize);
        try
        {
            _feedEntryService.CurrentUser = CurrentUser;
            var feedEntries = await _feedEntryService.GetFeedEntriesPaginated(
                currentPages + 1, _maxFeedEntryPageSize);
            //feedEntries.Reverse();
            FeedEntryCollection.AddRange(feedEntries);
        }
        catch //(Exception ex) 
        {
            //not too bad, possibly just no more ideas
            return;
        }
    }

    [RelayCommand]
    public Task ChangeCategoryPreference()
    {
        return Task.CompletedTask;
    }
}
