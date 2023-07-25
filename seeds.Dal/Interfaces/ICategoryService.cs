using seeds.Dal.Dto.FromDb;

namespace seeds.Dal.Interfaces;

public interface ICategoryService
{
    /* If no categories are found, throws.
     */
    public Task<List<CategoryFromDb>> GetCategoriesAsync();
    /* If no category found, throws.
     */
    public Task<CategoryFromDb> GetCategoryByKeyAsync(string categoryKey);
}
