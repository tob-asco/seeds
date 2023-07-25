using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
namespace seeds.Dal.Services;

public class CategoryService : ICategoryService 
{
    private readonly IDalBaseService _baseService;
    public CategoryService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<List<CategoryFromDb>> GetCategoriesAsync()
    {
        string url = $"api/Categories";
        var baseResult = await _baseService.GetDalModelAsync<List<CategoryFromDb>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<CategoryFromDb> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{categoryKey}";
        var baseResult = await _baseService.GetDalModelAsync<CategoryFromDb>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
