using Microsoft.Maui;
using Microsoft.Maui.Controls;
using seeds1.Interfaces;

namespace seeds1;

public partial class App : Application
{
    private readonly IStaticService stat;

    public App(
        IStaticService stat,
        AppShell shell)
	{
		InitializeComponent();
		MainPage = shell;
        this.stat = stat;
    }

    protected async override void OnStart()
    {
        base.OnStart();

        try
        {
            // load static resources into RAM
            await stat.LoadStaticsAsync();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Loading Static Data Error", ex.Message, "Ok");
        }
    }

#if WINDOWS
    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        const int newWidth = 500;
        const int newHeight = 800;

        window.Width = newWidth;
        window.Height = newHeight;

        return window;
    }
#endif
}
