using seeds.Dal.Wrappers;

namespace seeds.Dal.Interfaces;

public interface IDalBaseService
{
    public IHttpClientWrapper HttpClientWrapper { get; }

    /* Returns Null only if the response is NotFound.
     * Other bad responses throw.
     */
    public Task<T?> GetDalModelAsync<T>(
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Exception ThrowGetNullException(
        string endpointUrl, params (string Key, string Value)[] queryParams);
    
    /* Returns false only if the response is NotFound.
     * Other bad responses throw.
     */
    public Task<bool> PutDalModelAsync<T>(T newModel,
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Exception ThrowPutNotFoundException(
        string endpointUrl, params (string Key, string Value)[] queryParams);

    /* Returns false only if the response is Conflict.
     * Other bad responses throw.
     */
    public Task<bool> PostDalModelBoolReturnAsync<T>(T model,
        string endpointUrl, params (string Key, string Value)[] queryParams);

    /* Returns ModelFromDb if success, otherwise throws.
     */
    public Task<FromDb> PostDalModelAsync<ToDb, FromDb>(ToDb toDbModel,
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Exception ThrowPostConflictException(
        string endpointUrl, params (string Key, string Value)[] queryParams);
    
    /* Returns false only if the response is NotFound.
     * Other bad responses throw.
     */
    public Task<bool> DeleteAsync(
        string endpointUrl, params (string Key, string Value)[] queryParams);
    public Exception ThrowDeleteNotFoundException(
        string endpointUrl, params (string Key, string Value)[] queryParams);

    /* All bad responses throw.
     */
    public Task<T?> GetNonDalModelAsync<T>(
        string endpointUrl, params (string Key, string Value)[] queryParams);
}
