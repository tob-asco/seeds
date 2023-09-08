using CommunityToolkit.Maui.Core.Extensions;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;
using seeds1.Interfaces;
using System.Collections.ObjectModel;

namespace seeds1.ViewModel;

public partial class FamilyPopupViewModel : ObservableObject
{
    private readonly IStaticService stat;
    public TopicFromDb ChosenTopic { get; set; } = null!;
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
                OnPropertyChanged(nameof(DisplayedTopics));
            }
        }
    }
    public ObservableCollection<TopicFromDb> DisplayedTopics
    {
        get
        {
            if (SearchString == null || SearchString == "")
            { return WholeFamily.Topics.ToObservableCollection(); }
            return WholeFamily.Topics.Where(t => t.Name.ToLower().Contains(SearchString.ToLower())).ToObservableCollection();
        }
    }

    [RelayCommand]
    public void SetChosenTopic(TopicFromDb topic)
    {
        ChosenTopic = topic;
    }
}
