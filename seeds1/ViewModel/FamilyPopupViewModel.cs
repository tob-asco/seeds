using CommunityToolkit.Maui.Core.Extensions;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using seeds1.Interfaces;
using System.Collections.ObjectModel;

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
    public FamilyFromDb WholeFamily { get; set; }
    string searchString = "";
    public string SearchString
    {
        get => searchString;
        set
        {
            if (value != searchString)
            {
                searchString = value;
                OnPropertyChanged(nameof(SearchString));
                OnPropertyChanged(nameof(DisplayedTags));
            }
        }
    }
    public ObservableCollection<TagFromDb> DisplayedTags
    {
        get
        {
            if (SearchString == null || SearchString == "")
            { return WholeFamily.Tags.ToObservableCollection(); }
            return WholeFamily.Tags.Where(t => t.Name.ToLower().Contains(SearchString.ToLower())).ToObservableCollection();
        }
    }

    [RelayCommand]
    public void SetChosenTag(TagFromDb tag)
    {
        ChosenTag = tag;
    }
}
