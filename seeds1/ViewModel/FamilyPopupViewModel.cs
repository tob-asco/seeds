using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class FamilyPopupViewModel : ObservableObject
{
    private readonly IStaticService stat;
    public TagFromDb ChosenTag { get; set; } = null!;
    public FamilyPopupViewModel(
        IStaticService stat)
    {
        this.stat = stat;
    }
    [ObservableProperty]
    FamilyFromDb family;

    [RelayCommand]
    public void SetChosenTag(TagFromDb tag)
    {
        ChosenTag = tag;
    }
}
