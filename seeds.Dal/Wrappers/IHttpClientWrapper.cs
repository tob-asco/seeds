namespace seeds.Dal.Wrappers;

public interface IHttpClientWrapper
{
    public Uri BaseAddress { get; set; }
    public Task<HttpResponseMessage> GetAsync(string url);
    public Task<HttpResponseMessage> PutAsync(string url, HttpContent httpContent);
    public Task<HttpResponseMessage> PostAsync(string url, HttpContent httpContent);
    public Task<HttpResponseMessage> DeleteAsync(string url);
}
