using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class IdeasService : IIdeasService
{
    private readonly IDalBaseService _baseService;
    public IdeasService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<Idea?> GetIdeaAsync(int id)
    {
        string url = $"api/Ideas/{id}";
        return await _baseService.GetDalModelAsync<Idea>(url);
    }
    public async Task<List<Idea>?> GetIdeasPaginatedAsync(int page, int maxPageSize)
    {
        string url = $"api/ideas/page/{page}/size/{maxPageSize}";
        return await _baseService.GetDalModelAsync<List<Idea>>(url);
    }
}
