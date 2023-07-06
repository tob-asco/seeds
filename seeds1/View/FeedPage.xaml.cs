namespace seeds1.View;

public partial class FeedPage : ContentPage
{
	private readonly FeedViewModel _vm;
	public FeedPage(FeedViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
		_vm = vm;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
		await _vm.CollectIdeasPaginated();
    }
}