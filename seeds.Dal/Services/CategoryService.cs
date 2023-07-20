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
    public async Task<CategoryDtoApi?> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{categoryKey}";
        var r = await _baseService.GetDalModelAsync<CategoryDtoApi>(url);
        return r;
    }
}
