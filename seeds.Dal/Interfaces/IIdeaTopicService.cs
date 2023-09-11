using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IIdeaTopicService
{
    /* Returns list of topics (possibly length 0)
     */
    public Task<List<TopicFromDb>> GetTopicsOfIdeaAsync(int ideaId);
    /* Can only succeed or throw.
      */
    public Task PostIdeaTopicAsync(IdeaTopic ideaTopic);
    /* Can only succeed or throw.
     */
    public Task DeleteIdeaTopicAsync(
        int ideaId, string catKey, string topicName);
}
