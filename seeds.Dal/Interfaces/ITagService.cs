using seeds.Dal.Dto.ToAndFromDb;

namespace seeds.Dal.Interfaces;

public interface ITagService
{
    /* If no tags are found, throws.
     */
    public Task<List<TagDto>> GetTagsAsync();
    /* If no category found, throws.
     */
    public Task<TagDto> GetTagAsync(string categoryKey, string name);
}
