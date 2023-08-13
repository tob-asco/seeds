using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using System.Web;

namespace seeds.Dal.Services;

public class IdeaTagService : IIdeaTagService
{
    private readonly IDalBaseService baseService;
    public IdeaTagService(
        IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<TagFromDb>> GetTagsOfIdeaAsync(int ideaId)
    {
        string url = $"api/IdeaTags/{ideaId}";
        return await baseService.GetDalModelAsync<List<TagFromDb>>(url)
            ?? throw baseService.ThrowGetNullException(url);
    }
    public async Task PostIdeaTagAsync(IdeaTag ideaTag)
    {
        string url = "api/IdeaTags";
        if(!await baseService.PostDalModelBoolReturnAsync(url, ideaTag))
        {
            throw baseService.ThrowPostConflictException(url);
        }
    }
    public async Task DeleteIdeaTagAsync(int ideaId, string catKey, string tagName)
    {
        string url = $"api/IdeaTags/" +
            $"{ideaId}/{HttpUtility.UrlEncode(catKey)}/{HttpUtility.UrlEncode(tagName)}";
        if(!await baseService.DeleteAsync(url))
        {
            throw baseService.ThrowDeleteNotFoundException(url);
        }
    }

}
