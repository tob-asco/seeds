namespace seeds1.View;

public partial class DetailPage : ContentPage
{
	private readonly DetailViewModel vm;
	public DetailPage(DetailViewModel vm)
	{
		InitializeComponent();
		this.vm = vm;
		BindingContext = vm;
	}

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

		await vm.LoadAndDisplayPresentation();
    }
}