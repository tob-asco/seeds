using seeds.Dal.Interfaces;
using seeds.Dal.Wrappers;
using System.ComponentModel;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class DalBaseService : IDalBaseService
{
    /* The following is a property (and no readonly field) because
     * I want other DAL Service classes to take the same HttpClient
     * and they access the present class through DI.
     */
    public IHttpClientWrapper HttpClientWrapper { get; }
    public DalBaseService(IHttpClientWrapper httpClientWrapper)
    {
        HttpClientWrapper = httpClientWrapper;
    }

    public async Task<T?> GetDalModelAsync<T>(string url)
    {
        var response = await HttpClientWrapper.GetAsync(url);

        // check if the DB has produced a NotFound
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound
            && response.Headers.TryGetValues("X-Error-Type", out var vals))
        {
            if (vals != null && vals.FirstOrDefault(s => s == "DbRecordNotFound") != null)
            {
                return default; // gives null for Dal model classes (cf. tests)
            }
        }
        // throw for any non-successful response, not caputerd by the above
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Get URL {url} returned non-successful " +
                $"HttpStatusCode {response.StatusCode}");
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }
    public Exception ThrowGetNullException(string url)
    { return new Exception($"The Get URL {url} returned null."); }

    public async Task<bool> PutDalModelAsync<T>(string url, T newModel)
    {
        var httpContent = JsonContent.Create(newModel);
        var response = await HttpClientWrapper.PutAsync(url, httpContent);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Put URL {url} returned non-successful " +
                $"HttpStatusCode {response.StatusCode}");
        }
        return true;
    }
    public Exception ThrowPutNotFoundException(string url)
    { return new Exception($"The Put URL {url} returned NotFound."); }

    public async Task<bool> PostDalModelBoolReturnAsync<T>(string url, T model)
    {
        var httpContent = JsonContent.Create(model);
        var response = await HttpClientWrapper.PostAsync(url, httpContent);
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Post URL {url} returned non-successful " +
                $"HttpStatusCode {response.StatusCode}. Were the only problem an existing" +
                $" equivalent {typeof(T)}, the status code were Conflict.");
        }
        return true;
    }
    public Exception ThrowPostConflictException(string url)
    { return new Exception($"The Post URL {url} conflicted."); }

    public async Task<FromDb> PostDalModelAsync<ToDb, FromDb>(string url, ToDb toDbModel)
    {
        var httpContent = JsonContent.Create(toDbModel);
        var response = await HttpClientWrapper.PostAsync(url, httpContent);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Post URL {url} returned non-successful " +
                $"HttpStatusCode {response.StatusCode}.");
        }
        return await response.Content.ReadFromJsonAsync<FromDb>()
            ?? throw new Exception($"The Post URL {url} returned null.");
    }

    public async Task<bool> DeleteAsync(string url)
    {
        var response = await HttpClientWrapper.DeleteAsync(url);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Delete URL {url} returned non-successful " +
                $"HttpStatusCode {response.StatusCode}");
        }
        return true;
    }
    public Exception ThrowDeleteNotFoundException(string url)
    { return new Exception($"The Delete URL {url} returned NotFound."); }

    public async Task<T?> GetNonDalModelAsync<T>(string url)
    {
        var response = await HttpClientWrapper.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Get URL {url} returned non-successful " +
                $"HttpStatusCode {response.StatusCode}");
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }

}
