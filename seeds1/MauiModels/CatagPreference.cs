namespace seeds1.MauiModels;

#nullable enable
public partial class CatagPreference : ObservableObject
{
    public string CategoryKey { get; set; } = "NoC";
    public string CategoryName { get; set; } = "No Category";
    public string? TagName { get; set; }

    [ObservableProperty]
    int preference;
}
