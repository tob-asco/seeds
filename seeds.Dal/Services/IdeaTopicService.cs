using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class IdeaTopicService : IIdeaTopicService
{
    private readonly IDalBaseService baseService;
    public IdeaTopicService(
        IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<TopicFromDb>> GetTopicsOfIdeaAsync(int ideaId)
    {
        string url = $"api/IdeaTopics/{ideaId}";
        return await baseService.GetDalModelAsync<List<TopicFromDb>>(url)
            ?? throw baseService.ThrowGetNullException(url);
    }
    public async Task PostIdeaTopicAsync(IdeaTopic ideaTopic)
    {
        string url = "api/IdeaTopics";
        if(!await baseService.PostDalModelBoolReturnAsync(url, ideaTopic))
        {
            throw baseService.ThrowPostConflictException(url);
        }
    }
    public async Task DeleteIdeaTopicAsync(int ideaId, string catKey, string topicName)
    {
        string url = $"api/IdeaTopics/" +
            $"{ideaId}/{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(topicName)}";
        if(!await baseService.DeleteAsync(url))
        {
            throw baseService.ThrowDeleteNotFoundException(url);
        }
    }

}
