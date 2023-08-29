using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IIdeaTagService
{
    /* Returns list of tags (possibly length 0)
     */
    public Task<List<TagFromDb>> GetTagsOfIdeaAsync(int ideaId);
    /* Can only succeed or throw.
      */
    public Task PostIdeaTagAsync(IdeaTag ideaTag);
    /* Can only succeed or throw.
     */
    public Task DeleteIdeaTagAsync(
        int ideaId, string catKey, string tagName);
}
