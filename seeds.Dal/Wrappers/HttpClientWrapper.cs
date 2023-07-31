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
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://z4bppc68-5282.uks1.devtunnels.ms/")
        };
        _httpClient.BaseAddress = new Uri("http://localhost:5282/");
    }
    public async Task<HttpResponseMessage> GetAsync(string url)
    {
        return await _httpClient.GetAsync(url);
    }

    public async Task<HttpResponseMessage> PutAsync(string url, HttpContent httpContent)
    {
        return await _httpClient.PutAsync(url, httpContent);
    }
    public async Task<HttpResponseMessage> PostAsync(string url, HttpContent httpContent)
    {
        return await _httpClient.PostAsync(url, httpContent);
    }
}
