using seeds.Dal.Wrappers;

namespace seeds.Dal.Interfaces;

public interface IDalBaseService
{
    public IHttpClientWrapper HttpClientWrapper { get; }
    /* Returns Null only if the response is NotFound.
     * Other bad response throw.
     */
    public Task<T?> GetDalModelAsync<T>(string url);
    /* Returns false only if the response is NotFound.
     * Other bad response throw.
     */
    public Task<bool> PutDalModelAsync<T>(string url, T newModel);
    /* Returns false only if the response is Conflict.
     * Other bad response throw.
     */
    public Task<bool> PostDalModelBoolReturnAsync<T>(string url, T model);
    /* Returns ModelFromDb if success, otherwise throws.
     */
    public Task<FromDb> PostDalModelAsync<ToDb,FromDb>(string url, ToDb toDbModel);
    /* All bad responses throw.
     */
    public Task<T?> GetNonDalModelAsync<T>(string url);
}
