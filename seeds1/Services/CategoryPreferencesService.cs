using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class CategoryPreferencesService : ICategoryPreferencesService
{
    private readonly IGlobalService globalService;
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;

    public CategoryPreferencesService(
        IGlobalService globalService,
        ICategoryService categoryService,
        ICategoryUserPreferenceService cupService)
    {
        this.globalService = globalService;
        this.categoryService = categoryService;
        this.cupService = cupService;
    }

    public async Task<List<CatPreference>> GetCatPreferencesAsync()
    {
        List<CatPreference> catPrefs = new();
        var cats = await categoryService.GetCategoriesAsync();
        if (cats.Count == 0) { throw new Exception("No Categories returned."); }
        foreach (var cat in cats)
        {
            var cup = await cupService.GetCategoryUserPreferenceAsync(
                cat.Key, globalService.CurrentUser.Username);
            catPrefs.Add(new()
            {
                Key = cat.Key,
                Name = cat.Name,
                Value = cup.Value
            });
        }
        return catPrefs;
    }
    public int StepCatPreference(int oldPreference)
    {
        if (oldPreference == 0) { return 1; }
        else if (oldPreference == 1) { return -1; }
        else { return 0; }
    }
}
