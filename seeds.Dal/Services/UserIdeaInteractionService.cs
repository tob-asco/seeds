using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;
using System.Web;

namespace seeds.Dal.Services;

public class UserIdeaInteractionService : IUserIdeaInteractionService
{
    private readonly IDalBaseService _baseService;
    public UserIdeaInteractionService(IDalBaseService baseService)
    {
        _baseService = baseService;
    }

    public async Task<List<UserIdeaInteraction>> GetIdeaInteractionsOfUserAsync(string username)
    {
        string url = $"api/UserIdeaInteractions/{HttpUtility.UrlEncode(username)}";
        return await _baseService.GetDalModelAsync<List<UserIdeaInteraction>>(url)
            ?? throw _baseService.ThrowGetNullException(url);
    }
    public async Task<UserIdeaInteraction?> GetUserIdeaInteractionAsync(
        string username, int ideaId)
    {
        string url = $"api/UserIdeaInteractions/{HttpUtility.UrlEncode(username)}/{ideaId}";
        return await _baseService.GetDalModelAsync<UserIdeaInteraction>(url);
    }
    public async Task<bool> PutUserIdeaInteractionAsync(string username, int ideaId, bool newUpvoted, bool newDownvoted)
    {
        string url = $"api/UserIdeaInteractions/{HttpUtility.UrlEncode(username)}/{ideaId}";
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
        return await _baseService.PostDalModelBoolReturnAsync<UserIdeaInteraction>(url, uii);
    }
    public async Task PostOrPutUserIdeaInteractionAsync(UserIdeaInteraction newUii)
    {
        if (await PostUserIdeaInteractionAsync(newUii)) { return; }
        if (await PutUserIdeaInteractionAsync(
            newUii.Username,
            newUii.IdeaId,
            newUii.Upvoted,
            newUii.Downvoted)) { return; }
        throw new Exception("Neither could we Post, nor Put the specified interaction" +
            $"of {newUii.Username} with the idea of ID {newUii.IdeaId}.");
    }
    public async Task<int> CountVotesAsync(int ideaId)
    {
        string upvotesUrl = $"api/UserIdeaInteractions/{ideaId}/upvotes";
        string downvotesUrl = $"api/UserIdeaInteractions/{ideaId}/downvotes";
        int upvotes = await _baseService.GetNonDalModelAsync<int>(upvotesUrl);
        int downvotes = await _baseService.GetNonDalModelAsync<int>(downvotesUrl);
        return upvotes - downvotes;
    }
}
