using seeds.Dal.Dto.ToAndFromDb;

namespace seeds.Dal.Interfaces;

public interface ICategoryService
{
    /* If no categories are found, throws.
     */
    public Task<List<CategoryDto>> GetCategoriesAsync();
    /* If no category found, throws.
     */
    public Task<CategoryDto> GetCategoryByKeyAsync(string categoryKey);
}
