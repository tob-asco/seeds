using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using System.Web;

namespace seeds.Dal.Services;

public class TopicService : ITopicService
{
    private readonly IDalBaseService baseService;

    public TopicService(IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<TopicFromDb>> GetTopicsAsync()
    {
        string url = $"api/Topics";
        var baseResult = await baseService.GetDalModelAsync<List<TopicFromDb>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<TopicFromDb> GetTopicAsync(string catKey, string name)
    {
        string url = $"api/Topics/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(name)}";
        var baseResult = await baseService.GetDalModelAsync<TopicFromDb>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
