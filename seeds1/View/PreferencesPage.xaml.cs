using seeds1.Services;

namespace seeds1.View;

public partial class PreferencesPage : ContentPage
{
    private PreferencesViewModel vm;
    private readonly INavigationService navigationService;

    public PreferencesPage(
        //PreferencesViewModel vm,
        INavigationService navigationService)
    {
        InitializeComponent();
        //BindingContext = vm;
        //this.vm = vm;
        this.navigationService = navigationService;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        // Always re-create the VM.
        vm = Application.Current.Handler.MauiContext.Services.GetService<PreferencesViewModel>();
        BindingContext = vm;
        await vm.GetCatPreferencesAsync();
    }
}