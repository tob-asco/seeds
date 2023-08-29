using Microsoft.Maui;
using Microsoft.Maui.Controls;
using seeds1.Interfaces;

namespace seeds1;

public partial class App : Application
{
    private readonly IStaticService staticService;

    public App(
        IStaticService staticService,
        AppShell shell)
	{
		InitializeComponent();
		MainPage = shell;
        this.staticService = staticService;
    }

    protected async override void OnStart()
    {
        base.OnStart();

        // load static resources into RAM
        await staticService.LoadCategoriesAsync();
        await staticService.LoadFamiliesAsync();
        await staticService.LoadTagsAsync();
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
