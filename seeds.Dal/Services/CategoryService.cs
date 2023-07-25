using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
namespace seeds.Dal.Services;

public class CategoryService : ICategoryService 
{
    private readonly IDalBaseService _baseService;
    public CategoryService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        string url = $"api/Categories";
        var baseResult = await _baseService.GetDalModelAsync<List<CategoryDto>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<CategoryDto> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{categoryKey}";
        var baseResult = await _baseService.GetDalModelAsync<CategoryDto>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
