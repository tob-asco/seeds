using seeds.Dal.Dto.ToAndFromDb;
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
    [ObservableProperty] //Source generator
    [NotifyPropertyChangedFor(nameof(IsNotBusy))] // was called "AlsoNotifyChangeFor"
    bool isBusy;

    public bool IsNotBusy => !IsBusy;


    public MyBaseViewModel(IGlobalService globalService)
    {
        this.globalService = globalService;
    }

    [RelayCommand]
    public async Task Logout()
    {
        CurrentUser = null!;
        await Shell.Current.GoToAsync("///LoginPage", false);
    }
}