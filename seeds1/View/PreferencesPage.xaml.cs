namespace seeds1.View;

public partial class PreferencesPage : ContentPage
{
	public PreferencesPage(PreferencesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}