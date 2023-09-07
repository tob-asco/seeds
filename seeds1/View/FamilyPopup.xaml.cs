using CommunityToolkit.Maui.Views;
using seeds.Dal.Model;

namespace seeds1.View;

public partial class FamilyPopup : Popup
{
    private readonly FamilyPopupViewModel vm;

	public FamilyPopup(
		FamilyPopupViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        this.vm = vm;
    }

    private async void Tag_Clicked(object sender, EventArgs e)
    {
        await this.CloseAsync(vm.ChosenTag);
    }
}