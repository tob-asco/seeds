using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class DalBaseService
{
    protected readonly IHttpClientWrapper _httpClientWrapper;
    public DalBaseService(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    public async Task<T?> GetDalModelAsync<T>(string url) where T : class
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            // All types of exceptions will land here, e.g.
            // timeout, no such user, server overload, ...
            // not sure if this is expected behaviour. (TODO)
            Console.Write(ex);
            return null;
        }
    }
    public async Task<bool> PutDalModelAsync<T>(string url, T newModel)
    {
        try
        {
            var httpContent = JsonContent.Create(newModel);
            var response = await _httpClientWrapper.PutAsync(url, httpContent);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Console.Write(ex);
            return false;
        }
    }
}
