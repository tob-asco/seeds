using MvvmHelpers;
using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;
using seeds1.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace seeds1.ViewModel;

//    ...     ( property here   ...   , queryId    ...      )]
[QueryProperty(nameof(CurrentUsername),nameof(User.Username))]
public partial class FeedViewModel : BasisViewModel
{
    private static readonly int _maxFeedEntryPageSize = 10;
    //private readonly User _currentUser;
    private readonly IFeedEntryService _feedEntryService;
    [ObservableProperty]
    ObservableRangeCollection<FeedEntry> feedEntryCollection;
    public FeedViewModel(IFeedEntryService feedEntryService)
    {
        _feedEntryService = feedEntryService;
        FeedEntryCollection = new();
    }

    [RelayCommand]
    public async Task CollectIdeasPaginated()
    {
        int currentCount;
        if (FeedEntryCollection == null) { currentCount = 0; }
        else { currentCount = FeedEntryCollection.Count; }

        int currentPages = (int)Math.Ceiling((decimal)currentCount / _maxFeedEntryPageSize);
        try
        {
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
}
