using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class CatagPreferencesService : ICatagPreferencesService
{
    private readonly IGlobalService globalService;
    private readonly ICategoryService categoryService;
    private readonly ICatagUserPreferenceService cupService;
    private readonly ITagService tagService;

    public CatagPreferencesService(
        IGlobalService globalService,
        ICategoryService categoryService,
        ICatagUserPreferenceService cupService,
        ITagService tagService)
    {
        this.globalService = globalService;
        this.categoryService = categoryService;
        this.cupService = cupService;
        this.tagService = tagService;
    }

    public async Task<List<CatagPreference>> GetCatagPreferencesAsync()
    {
        List<CatagPreference> catagPrefs = new();
        var cats = await categoryService.GetCategoriesAsync();
        if (cats.Count == 0) { throw new Exception("No Categories returned."); }
        foreach (var cat in cats)
        {
            var cup = await cupService.GetCatagUserPreferenceAsync(
                cat.Key, globalService.CurrentUser.Username);
            catagPrefs.Add(new()
            {
                CategoryKey = cat.Key,
                CategoryName = cat.Name,
                Preference = cup.Value
            });
        }
        var tags = await tagService.GetTagsAsync();
        if (tags.Count == 0) { throw new Exception("No Tags returned."); }
        foreach (var tag in tags)
        {
            var tagsCat = cats.First(c => c.Key == tag.CategoryKey);
            var cup = await cupService.GetCatagUserPreferenceAsync(
                tag.CategoryKey, globalService.CurrentUser.Username, tag.Name);
            catagPrefs.Add(new()
            {
                CategoryKey = tag.CategoryKey,
                CategoryName = tagsCat.Name,
                TagName = tag.Name,
                Preference = cup.Value
            });
        }
        return catagPrefs;
    }
    public int StepPreference(int oldPreference)
    {
        if (oldPreference == 0) { return 1; }
        else if (oldPreference == 1) { return -1; }
        else { return 0; }
    }
}
