using seeds.Dal.Dto.ToApi;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class BasisViewModel : ObservableObject //partial because of source generation
{
    private readonly IGlobalVmService globalService;
    public UserDtoApi CurrentUser
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


    public BasisViewModel(IGlobalVmService globalService)
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