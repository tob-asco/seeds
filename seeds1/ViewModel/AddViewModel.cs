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
        IStaticService staticService,
        IGlobalService globalService,
        ICategoryService categoryService,
        IIdeasService ideasService,
        IPresentationService presentationService)
        : base(staticService, globalService)
    {
        this.categoryService = categoryService;
        this.ideasService = ideasService;
        this.presentationService = presentationService;
    }

    [ObservableProperty]
    ObservableRangeCollection<CategoryDto> cats = new();
    [ObservableProperty]
    CategoryDto pickedCat;
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
            PickedCat ??= Cats.FirstOrDefault(c => c.Key == "NoC");
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
                CategoryKey = PickedCat.Key,
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
