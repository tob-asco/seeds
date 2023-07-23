using seeds.Dal.Dto.ToApi;

namespace seeds.Dal.Interfaces;

public interface ICategoryService
{
    /* If no categories are found, throws.
     */
    public Task<List<CategoryDtoApi>> GetCategoriesAsync();
    /* If no category found, throws.
     */
    public Task<CategoryDtoApi?> GetCategoryByKeyAsync(string categoryKey);
}
