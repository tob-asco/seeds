using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class AddViewModel : MyBaseViewModel
{
    private readonly IIdeasService ideasService;

    public AddViewModel(
        IGlobalService globalService,
        IIdeasService ideasService)
        : base(globalService)
    {
        this.ideasService = ideasService;
    }

    [ObservableProperty]
    string enteredTitle, enteredSlogan, enteredDescription;

    [RelayCommand]
    public async Task AddIdea()
    {
        try
        {
            
        }
    }
}
