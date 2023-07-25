using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.ViewModel;

[QueryProperty(nameof(FeedEntry), nameof(FeedEntry))]
public partial class DetailViewModel : MyBaseViewModel
{
    private readonly IPresentationService presentationService;

    public FeedEntry FeedEntry { get; set; }
    public DetailViewModel(
        IGlobalService globalService,
        IPresentationService presentationService)
        : base(globalService)
    {
        this.presentationService = presentationService;
    }

    [ObservableProperty]
    string displayedDescription = "";

    [RelayCommand]
    public async Task LoadAndDisplayPresentation()
    {
        /* Call OnNavigatedTo
         */
        DisplayedDescription = "";
        seeds.Dal.Model.Presentation presi;
        try
        {
            presi = await presentationService.GetPresentationByIdeaIdAsync(FeedEntry.Idea.Id);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Error", ex.Message, "Ok");
            return;
        }
        if (presi == null)
        {
            DisplayedDescription = "No description provided.";
            return;
        }
        DisplayedDescription = presi.Description;
    }
}
