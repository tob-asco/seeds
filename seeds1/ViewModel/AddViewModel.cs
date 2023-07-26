using MvvmHelpers;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class AddViewModel : MyBaseViewModel
{
    private readonly ICategoryService categoryService;
    private readonly IIdeasService ideasService;
    private readonly IPresentationService presentationService;

    public AddViewModel(
        IGlobalService globalService,
        ICategoryService categoryService,
        IIdeasService ideasService,
        IPresentationService presentationService)
        : base(globalService)
    {
        this.categoryService = categoryService;
        this.ideasService = ideasService;
        this.presentationService = presentationService;
    }

    [ObservableProperty]
    ObservableRangeCollection<CategoryDto> cats;
    [ObservableProperty]
    string enteredTitle, enteredSlogan, enteredDescription;
    [ObservableProperty]
    string buttonText = "Add";

    [RelayCommand]
    public async Task PopulateCategories()
    {
        try
        {
            if (Cats == null || Cats?.Count == 0)
            {
                Cats.AddRange(await categoryService.GetCategoriesAsync());
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Error", ex.Message, "Ok");
        }
    }
    [RelayCommand]
    public async Task AddIdea()
    {
        ButtonText = "Adding...";
        try
        {
            IdeaFromDb idea = await ideasService.PostIdeaAsync(new()
            {
                Title = EnteredTitle,
                Slogan = EnteredSlogan,
                CreatorName = CurrentUser.Username,
            });
            EnteredTitle = "";
            EnteredSlogan = "";
            await presentationService.PostPresentationAsync(new()
            {
                IdeaId = idea.Id,
                Description = EnteredDescription,
            });
            EnteredDescription = "";
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Error", ex.Message, "Ok");
        }
        ButtonText = "Add";
    }
}
