using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Model;
using seeds1.Interfaces;
using System.ComponentModel;

namespace seeds1.ViewModel;

public partial class MyBaseViewModel : ObservableObject //partial because of source generation
{
    private readonly IStaticService stat;
    private readonly IGlobalService glob;

    UserDto currentUser;
    public UserDto CurrentUser => currentUser;

    [ObservableProperty] //Source generator
    [NotifyPropertyChangedFor(nameof(IsNotBusy))] // was called "AlsoNotifyChangeFor"
    bool isBusy;

    public bool IsNotBusy => !IsBusy;


    public MyBaseViewModel(
        IStaticService stat,
        IGlobalService glob)
    {
        this.stat = stat;
        this.glob = glob;
        glob.PropertyChanged += OnGlobPropertyChanged;
        //currentUser = glob.CurrentUser;
    }

    private void OnGlobPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(glob.CurrentUser))
        {
            if (CurrentUser != glob.CurrentUser)
            {
                currentUser = glob.CurrentUser;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
    }
}