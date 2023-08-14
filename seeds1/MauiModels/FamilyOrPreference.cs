namespace seeds1.MauiModels;

public partial class FamilyOrPreference : ObservableObject
{
    [ObservableProperty]
    public bool isFamily = false;
    [ObservableProperty]
    public TagFamily family = new();
    [ObservableProperty]
    public CatagPreference catagPreference = new();

}
