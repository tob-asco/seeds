using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class MyBaseViewModel : ObservableObject //partial because of source generation
{
    private readonly IGlobalService globalService;
    public UserDto CurrentUser
    {
        get => globalService.CurrentUser;
        set
        {
            globalService.CurrentUser = value;
            OnPropertyChanged(nameof(CurrentUser));
        }
    }
    public Dictionary<Guid, UserPreference> PreferencesDict
    {
        get => globalService.GetPreferences();
        set
        {
            globalService.CurrentUserPreferences = value;
            OnPropertyChanged(nameof(PreferencesDict));
        }
    }
    public Dictionary<int, UserIdeaInteraction> IdeaInteractionsDict
    {
        get => globalService.GetIdeaInteractions();
        set
        {
            globalService.CurrentUserIdeaInteractions = value;
            OnPropertyChanged(nameof(IdeaInteractionsDict));
        }
    }
    [ObservableProperty] //Source generator
    [NotifyPropertyChangedFor(nameof(IsNotBusy))] // was called "AlsoNotifyChangeFor"
    bool isBusy;

    public bool IsNotBusy => !IsBusy;


    public MyBaseViewModel(IGlobalService globalService)
    {
        this.globalService = globalService;
    }
}