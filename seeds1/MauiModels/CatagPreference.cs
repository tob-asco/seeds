using seeds.Dal.Dto.FromDb;

namespace seeds1.MauiModels;

#nullable enable
public partial class CatagPreference : ObservableObject
{
    public TagFromDb Tag { get; set; } = new();

    [ObservableProperty]
    int preference;
}
