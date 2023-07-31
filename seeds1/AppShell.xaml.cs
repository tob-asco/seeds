using seeds1.Interfaces;
using seeds1.View;
namespace seeds1;

public partial class AppShell : Shell
{
    private readonly IGlobalService globalService;

    public AppShell(IGlobalService globalService)
	{
		InitializeComponent();

        // Here we register routes that aren't visible in the flyout
        Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
        this.globalService = globalService;
    }
    private async void LogoutButton_Click(object sender, EventArgs e)
    {
        globalService.CurrentUser = null!;
        await Shell.Current.GoToAsync("///LoginPage", false);
    }
}
