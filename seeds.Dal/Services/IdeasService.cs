using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Interfaces;

namespace seeds.Dal.Services;

public class IdeasService : IIdeasService
{
    private readonly IDalBaseService _baseService;
    public IdeasService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<IdeaFromDb> GetIdeaAsync(int id)
    {
        string url = $"api/Ideas/{id}";
        return await _baseService.GetDalModelAsync<IdeaFromDb>(url)
            ?? throw new Exception($"The Get URL {url} returned null.");
    }
    public async Task<List<IdeaFromDb>?> GetIdeasPaginatedAsync(int page, int maxPageSize)
    {
        string url = $"api/ideas/page/{page}/size/{maxPageSize}";
        return await _baseService.GetDalModelAsync<List<IdeaFromDb>>(url);
    }
    public async Task PostIdeaAsync(IdeaToDb idea)
    {
        string url = "api/Ideas";
        if (!await _baseService.PostDalModelAsync<IdeaToDb>(url, idea))
        {
            throw new Exception($"The Post URL {url} returned false.");
        }
    }
}
