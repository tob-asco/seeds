using System.Runtime.CompilerServices;

namespace seeds1.View;

public partial class FeedPage : ContentPage
{
    private readonly FeedViewModel _vm;
    public FeedPage(FeedViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
        _vm = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        // add first page of feed entries.
        // here, not in OnAppearing, because otherwise CurrentUser were null
        if (_vm != null && _vm.RedrawPage)
        {
            _vm.FeedEntryCollection = new();
            await _vm.CollectFeedEntriesPaginated();
        }
    }
}