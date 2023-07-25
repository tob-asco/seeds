using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IPresentationService
{
    /* may return null
     */
    public Task<Presentation?> GetPresentationByIdeaIdAsync(int ideaId);
    /* If successful returns true,
      * if NotFound returns false,
      * else: base throws.
      */
    public Task<bool> PutPresentationAsync(int id, Presentation presi);
    /* If successful returns true,
      * if Conflict returns false,
      * else: base throws.
      */
    public Task<bool> PostPresentationAsync(Presentation presi);
    /* Can only succeed or throw.
     */
    public Task PostOrPutPresentationAsync(Presentation presi);

}
