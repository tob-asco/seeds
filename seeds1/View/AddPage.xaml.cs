namespace seeds1.View;

public partial class AddPage : ContentPage
{
	public AddPage(AddViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}