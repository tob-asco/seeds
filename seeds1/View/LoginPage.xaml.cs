namespace seeds1.View;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}