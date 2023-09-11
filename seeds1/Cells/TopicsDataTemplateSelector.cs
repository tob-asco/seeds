using seeds1.MauiModels;

namespace seeds1.Cells;

public class TopicsDataTemplateSelector : DataTemplateSelector
{
    public DataTemplate FamilyTemplate { get; set; }
    public DataTemplate PreferenceTemplate { get; set; }
    protected override DataTemplate OnSelectTemplate(
        object item, BindableObject container)
    {
        var fop = (FamilyOrPreference)item;
        return fop.IsFamily ? FamilyTemplate : PreferenceTemplate;
    }
}
