using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class CategoryUserPreferenceService : ICategoryUserPreferenceService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    public CategoryUserPreferenceService(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    public async Task<CategoryUserPreference> GetCategoryUserPreferenceAsync(
        string categoryKey, string username)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync(
                $"api/CategoryUserPreferences/{username}/{categoryKey}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CategoryUserPreference>()
                .ConfigureAwait(false) ?? throw new NullReferenceException();
        }
        catch (Exception ex)
        {
            // All types of exceptions will land here, e.g.
            // timeout, no such user, server overload, ...
            // not sure if this is expected behaviour. (TODO)
            return await Task.FromException<CategoryUserPreference>(ex)
                .ConfigureAwait(false);
        }
    }
}
