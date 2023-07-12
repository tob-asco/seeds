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
                $"api/CategoryUserPreferences/{categoryKey}/{username}");
            response.EnsureSuccessStatusCode(); // important, otherwise it will use std. model
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

    // returns true if HttpStatusCode is a successful one
    public async Task<bool> PutCategoryUserPreferenceAsync(string categoryKey, string username, int newPreference)
    {
        try
        {
            var httpContent = JsonContent.Create(new CategoryUserPreference
            {
                CategoryKey = categoryKey,
                Username = username,
                Value = newPreference
            });
            var response = await _httpClientWrapper.PutAsync(
                $"api/CategoryUserPreferences/{categoryKey}/{username}",
                httpContent);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return false;
        }
    }
}
