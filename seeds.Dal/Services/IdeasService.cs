using seeds.Dal.Interfaces;
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
    public async Task<Idea> GetIdeaAsync(int id)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync($"api/Ideas/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Idea>()
                .ConfigureAwait(false) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return await Task.FromException<Idea>(ex).ConfigureAwait(false);
        }
    }
    public async Task<List<Idea>> GetIdeasPaginatedAsync(int page, int maxPageSize)
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
    public async Task<bool> VoteIdeaAsync(int id, int updown)
    {
        try
        {
            var idea = await GetIdeaAsync(id);
            if (updown == +1) { idea.Upvotes++; }
            else if (updown == -1) { idea.Upvotes--; }
            else { return false; }
            await _httpClientWrapper.PutAsync($"api/Ideas/{id}", JsonContent.Create(idea));
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
