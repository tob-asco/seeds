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

    public async Task<T?> GetDalModelAsync<T>(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        var response = await HttpClientWrapper.GetAsync(endpointUrl, queryParams);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return default; // gives null for Dal model classes (cf. tests)
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Get URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned non-successful HttpStatusCode {response.StatusCode}");
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }
    public Exception ThrowGetNullException(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return new Exception($"The Get URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned null.");
    }

    public async Task<bool> PutDalModelAsync<T>(
        T newModel,
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        var httpContent = JsonContent.Create(newModel);
        var response = await HttpClientWrapper.PutAsync(httpContent, endpointUrl, queryParams);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Put URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned non-successful HttpStatusCode {response.StatusCode}");
        }
        return true;
    }
    public Exception ThrowPutNotFoundException(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return new Exception($"The Put URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned NotFound.");
    }

    public async Task<bool> PostDalModelBoolReturnAsync<T>(
        T model,
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        var httpContent = JsonContent.Create(model);
        var response = await HttpClientWrapper.PostAsync(httpContent, endpointUrl, queryParams);
        if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Post URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned non-successful HttpStatusCode {response.StatusCode}." +
                $" Were the only problem an existing" +
                $" equivalent {typeof(T)}, the status code were Conflict.");
        }
        return true;
    }
    public Exception ThrowPostConflictException(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return new Exception($"The Post URL " +
            $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)} conflicted.");
    }

    public async Task<FromDb> PostDalModelAsync<ToDb, FromDb>(
        ToDb toDbModel,
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        var httpContent = JsonContent.Create(toDbModel);
        var response = await HttpClientWrapper.PostAsync(
            httpContent, endpointUrl, queryParams);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Post URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned non-successful HttpStatusCode {response.StatusCode}.");
        }
        return await response.Content.ReadFromJsonAsync<FromDb>()
            ?? throw new Exception($"The Post URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned null.");
    }

    public async Task<bool> DeleteAsync(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        var response = await HttpClientWrapper.DeleteAsync(endpointUrl, queryParams);
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Delete URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned non-successful HttpStatusCode {response.StatusCode}");
        }
        return true;
    }
    public Exception ThrowDeleteNotFoundException(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return new Exception($"The Delete URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned NotFound.");
    }

    public async Task<T?> GetNonDalModelAsync<T>(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        var response = await HttpClientWrapper.GetAsync(endpointUrl, queryParams);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"The Get URL " +
                $"{HttpClientWrapper.BuildFullUrl(endpointUrl, queryParams)}" +
                $" returned non-successful HttpStatusCode {response.StatusCode}");
        }
        return await response.Content.ReadFromJsonAsync<T>();
    }

}
