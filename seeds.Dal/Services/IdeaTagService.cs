using seeds.Dal.Interfaces;
using seeds.Dal.Model;

namespace seeds.Dal.Services;

public class IdeaTagService : IIdeaTagService
{
    private readonly IDalBaseService baseService;
    public IdeaTagService(
        IDalBaseService baseService)
    {
        this.baseService = baseService;
    }
    public async Task<List<IdeaTag>> GetTagsOfIdeaAsync(int ideaId)
    {
        string url = $"api/IdeaTags/{ideaId}";
        return await baseService.GetDalModelAsync<List<IdeaTag>>(url)
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
        string url = $"api/IdeaTags/{ideaId}/{catKey}/{tagName}";
        if(!await baseService.DeleteAsync(url))
        {
            throw baseService.ThrowDeleteNotFoundException(url);
        }
    }

}
