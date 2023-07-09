using seeds.Dal.Model;

namespace seeds1.ViewModel;

public partial class BasisViewModel : ObservableObject //partial because of source generation
{
    public bool RedrawPage { get; set; } = false;
    [ObservableProperty] //Source generator
    User currentUser;

    [ObservableProperty] //Source generator
    //[NotifyPropertyChangedFor(nameof(IsNotBusy))] // was called "AlsoNotifyChangeFor"
    bool isBusy;

    //public bool IsNotBusy => !IsBusy;

    [RelayCommand]
    public async Task Logout()
    {
        await Shell.Current.GoToAsync("///LoginPage",false);
    }
}