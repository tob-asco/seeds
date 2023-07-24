using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
namespace seeds.Dal.Services;

public class CategoryService : ICategoryService 
{
    private readonly IDalBaseService _baseService;
    public CategoryService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<List<CategoryDtoApi>> GetCategoriesAsync()
    {
        string url = $"api/Categories";
        var baseResult = await _baseService.GetDalModelAsync<List<CategoryDtoApi>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<CategoryDtoApi> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{categoryKey}";
        var baseResult = await _baseService.GetDalModelAsync<CategoryDtoApi>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
