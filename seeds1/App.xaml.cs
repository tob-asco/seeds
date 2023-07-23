using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace seeds1;

public partial class App : Application
{
	public App(BaseViewModel vm)
	{
		InitializeComponent();

		MainPage = new AppShell(vm);
	}
}
