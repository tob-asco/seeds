using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class MyBaseViewModel : ObservableObject //partial because of source generation
{
    private readonly IStaticService staticService;
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


    public MyBaseViewModel(
        IStaticService staticService,
        IGlobalService globalService)
    {
        this.staticService = staticService;
        this.globalService = globalService;
    }
}