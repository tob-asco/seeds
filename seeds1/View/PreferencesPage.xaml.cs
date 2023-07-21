namespace seeds1.View;

public partial class PreferencesPage : ContentPage
{
    private readonly PreferencesViewModel vm;

    public PreferencesPage(PreferencesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        this.vm = vm;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        if (vm != null) { await vm.GetCatPreferencesAsync(); }
    }
}