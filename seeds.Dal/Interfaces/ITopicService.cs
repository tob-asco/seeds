using seeds.Dal.Dto.FromDb;

namespace seeds.Dal.Interfaces;

public interface ITopicService
{
    /* If no topics are found, throws.
     */
    public Task<List<TopicFromDb>> GetTopicsAsync();
    /* If no category found, throws.
     */
    public Task<TopicFromDb> GetTopicAsync(string categoryKey, string name);
}
