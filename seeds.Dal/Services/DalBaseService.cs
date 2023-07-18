using seeds.Dal.Interfaces;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class DalBaseService : IDalBaseService
{
    public IHttpClientWrapper HttpClientWrapper { get; }
    public DalBaseService(IHttpClientWrapper httpClientWrapper)
    {
        HttpClientWrapper = httpClientWrapper;
    }
    public async Task<T?> GetDalModelAsync<T>(string url)
    {
        try
        {
            var response = await HttpClientWrapper.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default(T); // should give null for Dal model classes
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
            var response = await HttpClientWrapper.PutAsync(url, httpContent);
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
            var response = await HttpClientWrapper.PostAsync(url, httpContent);
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
