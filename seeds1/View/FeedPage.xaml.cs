using seeds1.Factories;
using seeds1.Interfaces;
using System.Runtime.CompilerServices;

namespace seeds1.View;

public partial class FeedPage : ContentPage
{
    private readonly IGenericFactory<FeedViewModel> vmFactory;
    private readonly INavigationService navigationService;
    private FeedViewModel vm;
    public FeedPage(
        IGenericFactory<FeedViewModel> vmFactory,
        INavigationService navigationService)
    {
        InitializeComponent();
        this.vmFactory = vmFactory;
        this.navigationService = navigationService;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (vm == null || BindingContext == null ||
            navigationService.RedrawNavigationTarget == true)
        {
            // create a new instance of the VM w/o calling its constructor:
            vm = vmFactory.Create();
            BindingContext = vm;
            if(vm.FeedEntryVMCollection == null || 
                vm.FeedEntryVMCollection?.Count == 0)
            {
                await vm.CollectFeedEntriesPaginated();
            }
            navigationService.RedrawNavigationTarget = false;
        }
        
        if (vm != null)
        {
            await vm.LoadCatPreferencesFromDbAsync();
        }
    }
}