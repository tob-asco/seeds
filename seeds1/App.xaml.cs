using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace seeds1;

public partial class App : Application
{
	public App(AppShell shell)
	{
		InitializeComponent();
		MainPage = shell;
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
