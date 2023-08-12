using seeds.Dal.Wrappers;

namespace seeds.Dal.Interfaces;

public interface IDalBaseService
{
    public IHttpClientWrapper HttpClientWrapper { get; }

    /* Returns Null only if the response is NotFound.
     * Other bad responses throw.
     */
    public Task<T?> GetDalModelAsync<T>(string url);
    public Exception ThrowGetNullException(string url);
    
    /* Returns false only if the response is NotFound.
     * Other bad responses throw.
     */
    public Task<bool> PutDalModelAsync<T>(string url, T newModel);
    public Exception ThrowPutNotFoundException(string url);

    /* Returns false only if the response is Conflict.
     * Other bad responses throw.
     */
    public Task<bool> PostDalModelBoolReturnAsync<T>(string url, T model);

    /* Returns ModelFromDb if success, otherwise throws.
     */
    public Task<FromDb> PostDalModelAsync<ToDb,FromDb>(string url, ToDb toDbModel);
    public Exception ThrowPostConflictException(string url);
    
    /* Returns false only if the response is NotFound.
     * Other bad responses throw.
     */
    public Task<bool> DeleteAsync(string url);
    public Exception ThrowDeleteNotFoundException(string url);

    /* All bad responses throw.
     */
    public Task<T?> GetNonDalModelAsync<T>(string url);
}
