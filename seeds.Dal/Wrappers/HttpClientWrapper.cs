using System.Web;

namespace seeds.Dal.Wrappers;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient httpClient;
    public Uri BaseAddress
    {
        get => httpClient.BaseAddress ?? new Uri("");
        set => httpClient.BaseAddress = value;
    }

    public HttpClientWrapper()
    {
        httpClient = new HttpClient
        {
            //BaseAddress = new Uri("https://z4bppc68-5282.uks1.devtunnels.ms/") // #1, not working
            //BaseAddress = new Uri("https://q73sqz83-5282.euw.devtunnels.ms/") // #2
            BaseAddress = new Uri("https://0cfsqc33-5282.euw.devtunnels.ms/") // test, working
        };
        //_httpClient.BaseAddress = new Uri("http://localhost:5282/");
    }
    public string BuildFullUrl(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        if (string.IsNullOrWhiteSpace(endpointUrl))
        {
            throw new ArgumentException(
                "Base URL cannot be empty or null.", nameof(endpointUrl));
        }

        if (queryParams == null || queryParams.Length == 0)
        {
            return endpointUrl;
        }

        var encodedParams = string.Join("&", queryParams
            .Select(param => $"{param.Key}={HttpUtility.UrlEncode(param.Value)}"));

        return $"{endpointUrl}?{encodedParams}";
    }

    public async Task<HttpResponseMessage> GetAsync(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return await httpClient.GetAsync(BuildFullUrl(endpointUrl, queryParams));
    }

    public async Task<HttpResponseMessage> PutAsync(
        HttpContent httpContent,
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return await httpClient.PutAsync(BuildFullUrl(endpointUrl, queryParams), httpContent);
    }
    public async Task<HttpResponseMessage> PostAsync(
        HttpContent httpContent,
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return await httpClient.PostAsync(BuildFullUrl(endpointUrl, queryParams), httpContent);
    }

    public async Task<HttpResponseMessage> DeleteAsync(
        string endpointUrl, params (string Key, string Value)[] queryParams)
    {
        return await httpClient.DeleteAsync(BuildFullUrl(endpointUrl, queryParams));
    }
}
