using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class CatPreferencesService : ICatPreferencesService
{
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;

    public UserDtoApi CurrentUser { get; set; }
    public CatPreferencesService(
        ICategoryService categoryService,
        ICategoryUserPreferenceService cupService)
    {
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
                    cat.Key, CurrentUser.Username);
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
}
