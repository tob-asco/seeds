using seeds1.Services;
using System.Runtime.CompilerServices;

namespace seeds1.View;

public partial class FeedPage : ContentPage
{
    private readonly INavigationService navigationService;
    private FeedViewModel _vm;
    public FeedPage(
        //FeedViewModel vm,
        INavigationService navigationService)
    {
        InitializeComponent();
        this.navigationService = navigationService;

        //BindingContext = vm;
        //_vm = vm;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (_vm == null || BindingContext == null ||
            navigationService.RedrawNavigationTarget == true)
        {
            // create a new instance of the VM w/o calling its constructor:
            _vm = Application.Current.Handler.MauiContext.Services.GetService<FeedViewModel>();
            BindingContext = _vm;
            if(_vm.FeedEntryVMCollection == null || 
                _vm.FeedEntryVMCollection?.Count == 0)
            {
                await _vm.CollectFeedEntriesPaginated();
            }
            navigationService.RedrawNavigationTarget = false;
        }
    }
}