using seeds.Dal.Interfaces;
using seeds.Dal.Model;
namespace seeds.Dal.Services;

public class CategoryService : ICategoryService 
{
    private readonly IDalBaseService _baseService;
    public CategoryService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<Category?> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{categoryKey}";
        var r = await _baseService.GetDalModelAsync<Category>(url);
        return r;
    }
}
