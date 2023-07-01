using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Wrappers;

public class HttpClientWrapper : IHttpClientWrapper
{
    private readonly HttpClient _httpClient;
    public Uri BaseAddress 
    { 
        get => _httpClient.BaseAddress ?? new Uri("");
        set => _httpClient.BaseAddress = value;
    }

    public HttpClientWrapper()
    {
        _httpClient = new HttpClient();
    }
    public Task<HttpResponseMessage> GetAsync(string url)
    {
        return _httpClient.GetAsync(url);
    }
}
