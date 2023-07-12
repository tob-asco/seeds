using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public class CategoryService : ICategoryService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    public CategoryService(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    public async Task<Category> GetCategoryByKeyAsync(string categoryKey)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync("api/Categories/" + categoryKey)
                                            .ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Category>()
                .ConfigureAwait(false) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            // All types of exceptions will land here, e.g.
            // timeout, no such user, server overload, ...
            // not sure if this is expected behaviour. (TODO)
            return await Task.FromException<Category>(ex).ConfigureAwait(false);
        }
    }
}
