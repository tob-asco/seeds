using seeds.Dal.Dto.FromDb;

namespace seeds.Dal.Interfaces;

public interface ITagService
{
    /* If no tags are found, throws.
     */
    public Task<List<TagFromDb>> GetTagsAsync();
    /* If no category found, throws.
     */
    public Task<TagFromDb> GetTagAsync(string categoryKey, string name);
}
