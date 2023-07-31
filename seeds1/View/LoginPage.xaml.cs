using seeds1.Factories;

namespace seeds1.View;

public partial class LoginPage : ContentPage
{
    private readonly IGenericFactory<LoginViewModel> factory;

    public LoginPage(
		IGenericFactory<LoginViewModel> factory)
	{
		InitializeComponent();
        this.factory = factory;
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        BindingContext = factory.Create();
    }
}