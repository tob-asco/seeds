using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class TagFamilyService : ITagFamilyService
{
    private readonly ICatagPreferencesService catagPrefService;

    public TagFamilyService(
        ICatagPreferencesService catagPrefService)
    {
        this.catagPrefService = catagPrefService;
    }
    public List<FamilyOrPreference> ConvertToFamilyOrPreferences(List<CatagPreference> catagPrefs)
    {
        List<FamilyOrPreference> fops = new();
        List<TagFamily> fams = new();
        foreach (var catagPref in catagPrefs)
        {
            if (catagPref.TagName != null && // otherwise this is for a category
                catagPref.TagName.Contains('|') && // the separator symbolising a family
                catagPref.Preference == 0)
            {
                string groupName = catagPref.TagName.Split('|')[0].Trim();
                TagFamily fam = fams.FirstOrDefault(f => f.Name == groupName);
                if (fam != null)
                {
                    // update fams by updating fam
                    fam.Tags.Add(new()
                    {
                        Name = catagPref.TagName,
                        CategoryKey = catagPref.CategoryKey,
                    });
                }
                else
                {
                    // add new fam and this tag to it
                    fams.Add(new()
                    {
                        Name = groupName,
                        Tags = new() { new()
                        {
                            Name = catagPref.TagName,
                            CategoryKey = catagPref.CategoryKey,
                        }}
                    });
                }
            }
            else
            {
                // this catagPref is no family member
                fops.Add(new()
                {
                    IsFamily = false,
                    CatagPreference = new()
                    {
                        CategoryKey = catagPref.CategoryKey,
                        CategoryName = catagPref.CategoryName,
                        TagName = catagPref.TagName,
                        Preference = catagPref.Preference,
                    }
                });
            }
        }
        foreach (var fam in fams)
        {
            fops.Add(new()
            {
                IsFamily = true,
                Family = fam
            });
        }
        return fops;
    }

    public async Task<List<FamilyOrPreference>> GetAllFamilyOrPreferenceAsync()
    {
        var catagPrefs = await catagPrefService.GetCatagPreferencesAsync();
        return ConvertToFamilyOrPreferences(catagPrefs);
    }
}
