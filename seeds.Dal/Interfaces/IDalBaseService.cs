using seeds.Dal.Wrappers;

namespace seeds.Dal.Interfaces;

public interface IDalBaseService
{
    public IHttpClientWrapper HttpClientWrapper { get; }
    /* Returns Null only if the response is NotFound.
     * Other bad response throw an exception.
     */
    public Task<T?> GetDalModelAsync<T>(string url);
    /* Returns false only if the response is NotFound.
     * Other bad response throw an exception.
     */
    public Task<bool> PutDalModelAsync<T>(string url, T newModel);
    /* Returns Null only if the response is Conflict.
     * Other bad response throw an exception.
     */
    public Task<bool> PostDalModelAsync<T>(string url, T model);
}
