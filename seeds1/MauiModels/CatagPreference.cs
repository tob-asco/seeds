using seeds.Dal.Dto.FromDb;

namespace seeds1.MauiModels;

#nullable enable
public partial class CatopicPreference : ObservableObject
{
    public TopicFromDb Topic { get; set; } = new();

    [ObservableProperty]
    int preference;
}
