using seeds1.MauiModels;

namespace seeds1.Cells;

public class FamilyOrPreferenceDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate FamilyTemplate { get; set; }
    public DataTemplate PreferenceTemplate { get; set; }
    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        var famOrPref = (FamilyOrPreference)item;
        return famOrPref.IsFamily ? FamilyTemplate : PreferenceTemplate;
    }
}
