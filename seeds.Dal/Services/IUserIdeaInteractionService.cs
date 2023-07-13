using seeds.Dal.Model;

namespace seeds.Dal.Services;

public interface IUserIdeaInteractionService
{
    public Task<UserIdeaInteraction> GetUserIdeaInteractionAsync(
        string username,
        int ideaId
        );
    // returns a bool of success
    public Task<bool> PutUserIdeaInteractionAsync(
        string username,
        int ideaId,
        bool newUpvoted,
        bool newDownvoted
        );
}
