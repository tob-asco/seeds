using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class CatPreferencesService : ICatPreferencesService
{
    private readonly IGlobalVmService globalService;
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;

    public CatPreferencesService(
        IGlobalVmService globalService,
        ICategoryService categoryService,
        ICategoryUserPreferenceService cupService)
    {
        this.globalService = globalService;
        this.categoryService = categoryService;
        this.cupService = cupService;
    }

    public async Task<IEnumerable<CatPreference>> GetCatPreferencesAsync()
    {
        List<CatPreference> catPrefs = new();
        var cats = await categoryService.GetCategoriesAsync();
        foreach (var cat in cats)
        {
            try
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
            catch (Exception ex)
            {
                // we will not have this category
                Console.WriteLine(ex);
            }
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
