using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
namespace seeds.Dal.Services;

public class CategoryService : DalBaseService, ICategoryService 
{
    public CategoryService(IHttpClientWrapper httpClientWrapper)
        : base(httpClientWrapper) { }
    public async Task<Category?> GetCategoryByKeyAsync(string categoryKey)
    {
        string url = $"api/Categories/{categoryKey}";
        return await GetDalModelAsync<Category>(url);
    }
}
