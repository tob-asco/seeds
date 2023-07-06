using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public class IdeasService : IIdeasService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    public IdeasService(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    public async Task<List<Idea>> GetIdeasPaginated(int page, int maxPageSize)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync($"api/ideas/page/{page}/size/{maxPageSize}")
                                            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<Idea>>()
                .ConfigureAwait(false) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return await Task.FromException<List<Idea>>(ex).ConfigureAwait(false);
        }
    }
}
