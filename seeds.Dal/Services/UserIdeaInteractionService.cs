using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class UserIdeaInteractionService : DalBaseService, IUserIdeaInteractionService
{
    public UserIdeaInteractionService(IHttpClientWrapper httpClientWrapper)
        : base(httpClientWrapper) { }
    public async Task<UserIdeaInteraction?> GetUserIdeaInteractionAsync(
        string username, int ideaId)
    {
        string url = $"api/UserIdeaInteractions/{username}/{ideaId}";
        return await GetDalModelAsync<UserIdeaInteraction>(url);
    }

    public async Task<bool> PutUserIdeaInteractionAsync(string username, int ideaId, bool newUpvoted, bool newDownvoted)
    {
        string url = $"api/UserIdeaInteractions/{username}/{ideaId}";
        UserIdeaInteraction newUii = new()
        {
            Username = username,
            IdeaId = ideaId,
            Downvoted = newDownvoted,
            Upvoted = newUpvoted
        };
        return await PutDalModelAsync<UserIdeaInteraction>(url, newUii);
    }
    public async Task<bool> PostUserIdeaInteractionAsync(UserIdeaInteraction uii)
    {
        string url = "api/UserIdeaInteractions";
        return await PostDalModelAsync<UserIdeaInteraction>(url, uii);
    }
    public async Task<bool> PostOrPutUserIdeaInteractionAsync(UserIdeaInteraction newUii)
    {
        if(await PostUserIdeaInteractionAsync(newUii)) return true;
        return await PutUserIdeaInteractionAsync(
            newUii.Username,
            newUii.IdeaId,
            newUii.Upvoted,
            newUii.Downvoted);
    }
    public async Task<int> CountVotesAsync(int ideaId)
    {
        string upvotesUrl = $"api/UserIdeaInteractions/{ideaId}/upvotes";
        string downvotesUrl = $"api/UserIdeaInteractions/{ideaId}/downvotes";
        int upvotes = await GetDalModelAsync<int>(upvotesUrl);
        int downvotes = await GetDalModelAsync<int>(downvotesUrl);
        return upvotes - downvotes; 
    }
}
