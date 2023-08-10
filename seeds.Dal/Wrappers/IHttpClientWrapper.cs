using System.Web;

namespace seeds.Dal.Wrappers;

public interface IHttpClientWrapper
{
    public Uri BaseAddress { get; set; }
    public string BuildFullUrl(
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Task<HttpResponseMessage> GetAsync(
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Task<HttpResponseMessage> PutAsync(
        HttpContent httpContent,
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Task<HttpResponseMessage> PostAsync(
        HttpContent httpContent,
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Task<HttpResponseMessage> DeleteAsync(
        string endpointUrl, params (string Key, string Value)[] queryParams);
}
