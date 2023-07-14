namespace seeds1.View;

public partial class FeedEntryView : ContentView
{
	public FeedEntryView(FeedEntryVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}