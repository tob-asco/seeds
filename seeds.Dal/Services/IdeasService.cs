using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class IdeasService : DalBaseService, IIdeasService
{
    public IdeasService(IHttpClientWrapper httpClientWrapper)
        : base(httpClientWrapper) { }
    public async Task<Idea?> GetIdeaAsync(int id)
    {
        string url = $"api/Ideas/{id}";
        return await GetDalModelAsync<Idea>(url);
    }
    public async Task<List<Idea>?> GetIdeasPaginatedAsync(int page, int maxPageSize)
    {
        string url = $"api/ideas/page/{page}/size/{maxPageSize}";
        return await GetDalModelAsync<List<Idea>>(url);
    }
}
