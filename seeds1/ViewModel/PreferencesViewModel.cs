using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using System.Collections.ObjectModel;

namespace seeds1.ViewModel;

[QueryProperty(nameof(CurrentUser), nameof(CurrentUser))] //available AFTER ctor, ...
[QueryProperty(nameof(RedrawPage),nameof(RedrawPage))]
public partial class PreferencesViewModel : BasisViewModel
{
    private readonly ICategoryService catService;
    private readonly ICategoryUserPreferenceService cupService;

    [ObservableProperty]
    ObservableCollection<CategoryDtoApi> cats;

    public PreferencesViewModel(
        ICategoryService catService,
        ICategoryUserPreferenceService cupService)
    {
        this.catService = catService;
        this.cupService = cupService;
    }

    [RelayCommand]
    public async Task GetCategoriesAync()
    {
        
    }
}
