using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using System.Web;

namespace seeds.Dal.Services;

public class CategoryService : ICategoryService 
{
    private readonly IDalBaseService baseService;
    public CategoryService(IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<CategoryDto>> GetCategoriesAsync()
    {
        string url = $"api/Categories";
        var baseResult = await baseService.GetDalModelAsync<List<CategoryDto>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<CategoryDto> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{HttpUtility.UrlEncode(categoryKey)}";
        var baseResult = await baseService.GetDalModelAsync<CategoryDto>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
