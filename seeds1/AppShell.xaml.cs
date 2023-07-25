using seeds1.View;
namespace seeds1;

public partial class AppShell : Shell
{
	public AppShell(MyBaseViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;

        // Here we register routes that aren't visible in the flyout
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
    }
}
