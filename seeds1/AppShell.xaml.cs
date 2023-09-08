using seeds1.Interfaces;
using seeds1.View;
namespace seeds1;

public partial class AppShell : Shell
{
    private readonly IGlobalService glob;

    public AppShell(IGlobalService glob)
	{
		InitializeComponent();

        // Here we register routes that aren't visible in the flyout
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        this.glob = glob;
    }
    private async void LogoutButton_Click(object sender, EventArgs e)
    {
        glob.Dispose();
        await Shell.Current.GoToAsync("///LoginPage", false);
    }
}
