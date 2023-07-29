using seeds1.View;
namespace seeds1;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Here we register routes that aren't visible in the flyout
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
    }
}
