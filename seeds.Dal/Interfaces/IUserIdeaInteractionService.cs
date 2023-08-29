using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IUserIdeaInteractionService
{
    public Task<List<UserIdeaInteraction>> GetIdeaInteractionsOfUserAsync(
        string username);
    /* May return null.
     */
    public Task<UserIdeaInteraction?> GetUserIdeaInteractionAsync(
        string username, int ideaId );
    /* If successful returns true,
      * if not-found returns false,
      * else: base throws.
      */
    public Task<bool> PutUserIdeaInteractionAsync(
        string username, int ideaId, bool newUpvoted, bool newDownvoted );
    /* If successful returns true,
      * if Conflict returns false,
      * else: base throws.
      */
    public Task<bool> PostUserIdeaInteractionAsync(UserIdeaInteraction uii);
    /* Can only succeed or throw.
     */
    public Task PostOrPutUserIdeaInteractionAsync(UserIdeaInteraction newUii);
    /* Can only succeed or base throws.
     */
    public Task<int> CountVotesAsync(int ideaId);
}
