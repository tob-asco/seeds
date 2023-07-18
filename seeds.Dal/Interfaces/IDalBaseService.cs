using seeds.Dal.Wrappers;

namespace seeds.Dal.Interfaces;

public interface IDalBaseService
{
    public IHttpClientWrapper HttpClientWrapper { get; }
    public Task<T?> GetDalModelAsync<T>(string url);
    public Task<bool> PutDalModelAsync<T>(string url, T newModel);
    public Task<bool> PostDalModelAsync<T>(string url, T model);
}
