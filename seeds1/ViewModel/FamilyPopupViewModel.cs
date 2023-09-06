using seeds.Dal.Model;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class FamilyPopupViewModel : ObservableObject
{
    private readonly IStaticService stat;

    [ObservableProperty]
    Family family;
    public FamilyPopupViewModel(
        IStaticService stat)
    {
        this.stat = stat;
    }
}
