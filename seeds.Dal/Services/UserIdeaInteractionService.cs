using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class UserIdeaInteractionService : IUserIdeaInteractionService
{
    private readonly IDalBaseService _baseService;
    public UserIdeaInteractionService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }
    public async Task<UserIdeaInteraction?> GetUserIdeaInteractionAsync(
        string username, int ideaId)
    {
        string url = $"api/UserIdeaInteractions/{username}/{ideaId}";
        return await _baseService.GetDalModelAsync<UserIdeaInteraction>(url);
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
        return await _baseService.PutDalModelAsync<UserIdeaInteraction>(url, newUii);
    }
    public async Task<bool> PostUserIdeaInteractionAsync(UserIdeaInteraction uii)
    {
        string url = "api/UserIdeaInteractions";
        return await _baseService.PostDalModelAsync<UserIdeaInteraction>(url, uii);
    }
    public async Task<bool> PostOrPutUserIdeaInteractionAsync(UserIdeaInteraction newUii)
    {
        if (await PostUserIdeaInteractionAsync(newUii)) return true;
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
        int upvotes = await _baseService.GetDalModelAsync<int>(upvotesUrl);
        int downvotes = await _baseService.GetDalModelAsync<int>(downvotesUrl);
        return upvotes - downvotes;
    }
}
