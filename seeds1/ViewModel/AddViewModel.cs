using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class AddViewModel : MyBaseViewModel
{
    private readonly IIdeasService ideasService;
    private readonly IPresentationService presentationService;

    public AddViewModel(
        IGlobalService globalService,
        IIdeasService ideasService,
        IPresentationService presentationService)
        : base(globalService)
    {
        this.ideasService = ideasService;
        this.presentationService = presentationService;
    }

    [ObservableProperty]
    string enteredTitle, enteredSlogan, enteredDescription;

    [RelayCommand]
    public async Task AddIdea()
    {
        try
        {
            IdeaFromDb idea = await ideasService.PostIdeaAsync(new()
            {
                Title = EnteredTitle,
                Slogan = EnteredSlogan,
                CreatorName = CurrentUser.Username,
            });
            await presentationService.PostPresentationAsync(new()
            {
                IdeaId = idea.Id,
                Description = EnteredDescription,
            });
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Error", ex.Message, "Ok");
        }
    }
}
