namespace seeds1.View;

public partial class FeedPage : ContentPage
{
	public FeedPage(FeedViewModel feedViewModel)
	{
		InitializeComponent();

		BindingContext = feedViewModel;
	}
}