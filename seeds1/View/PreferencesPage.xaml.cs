using seeds1.Factories;
using seeds1.Interfaces;

namespace seeds1.View;

public partial class PreferencesPage : ContentPage
{
    private readonly IGenericFactory<PreferencesViewModel> vmFactory;
    private PreferencesViewModel vm;

    public PreferencesPage(
        IGenericFactory<PreferencesViewModel> vmFactory)
    {
        InitializeComponent();
        this.vmFactory = vmFactory;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        // Always re-create the VM.
        vm = vmFactory.Create();
        BindingContext = vm;
        await vm.PopulateListListAsync();
    }
}