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
    public async Task<T?> GetDalModelAsync<T>(string url)// where T : class
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(T); // my code for "not found"
                                   // I hope it gives null for DAL model classes
                                   // TODO: test this.
            }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception ex)
        {
            // All non-successful response messages other than 4xx (not found)
            // will land here, and they are bad.
            Console.Write(ex);
            throw;
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
    public async Task<bool> PostDalModelAsync<T>(string url, T model)
    {
        try
        {
            var httpContent = JsonContent.Create(model);
            var response = await _httpClientWrapper.PostAsync(url, httpContent);
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
