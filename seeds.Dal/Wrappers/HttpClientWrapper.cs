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
            //BaseAddress = new Uri("http://localhost:5282/"); //my localhost address
            //BaseAddress = new Uri("https://z4bppc68-5282.uks1.devtunnels.ms/") // #1, not working
            //BaseAddress = new Uri("https://q73sqz83-5282.euw.devtunnels.ms/") // #2, not working
            BaseAddress = new Uri("https://0cfsqc33-5282.euw.devtunnels.ms/") // working!!!
        };
    }
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await httpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent httpContent)
    {
        return await httpClient.PutAsync(url, httpContent);
    }
    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent httpContent)
    {
        return await httpClient.PostAsync(url, httpContent);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        return await httpClient.DeleteAsync(url);
    }
}
