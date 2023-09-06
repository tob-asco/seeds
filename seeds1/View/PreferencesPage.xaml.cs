using CommunityToolkit.Maui.Views;
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
    //private async void FamilyPopUp_Click(object sender, EventArgs e)
    //{
    //    var popup = new FamilyPopup();

    //    await this.ShowPopupAsync(popup);
    //}
}