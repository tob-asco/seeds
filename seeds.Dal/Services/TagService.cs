using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using System.Web;

namespace seeds.Dal.Services;

public class TagService : ITagService
{
    private readonly IDalBaseService baseService;

    public TagService(IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<TagFromDb>> GetTagsAsync()
    {
        string url = $"api/Tags";
        var baseResult = await baseService.GetDalModelAsync<List<TagFromDb>>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
    public async Task<TagFromDb> GetTagAsync(string catKey, string name)
    {
        string url = $"api/Tags/" +
            $"{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(name)}";
        var baseResult = await baseService.GetDalModelAsync<TagFromDb>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
        return baseResult;
    }
}
