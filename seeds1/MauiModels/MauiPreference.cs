using seeds.Dal.Dto.FromDb;

namespace seeds1.MauiModels;

#nullable enable
public partial class MauiPreference : ObservableObject
{
    public TopicFromDb Topic { get; set; } = new();

    [ObservableProperty]
    int preference;
}
