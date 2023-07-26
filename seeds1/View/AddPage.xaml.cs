namespace seeds1.View;

public partial class AddPage : ContentPage
{
    private readonly AddViewModel vm;

    public AddPage(AddViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        this.vm = vm;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await vm.PopulateCategories();
    }
}