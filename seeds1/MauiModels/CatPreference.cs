namespace seeds1.MauiModels;

public partial class CatPreference : ObservableObject
{
    public string Key { get; set; } = "NoC";
    public string Name { get; set; } = "No Category";
    [ObservableProperty]
    int value;
}
