using seeds1.View;
namespace seeds1;

public partial class AppShell : Shell
{
	public AppShell(BaseViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        // HT: here we register routes that aren't visible in the flyout
        //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
    }
}
