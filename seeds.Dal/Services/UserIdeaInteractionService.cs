using seeds.Dal.Model;
using seeds.Dal.Wrappers;
using System.Net.Http.Json;

namespace seeds.Dal.Services;

public class UserIdeaInteractionService : IUserIdeaInteractionService
{
    private readonly IHttpClientWrapper _httpClientWrapper;
    public UserIdeaInteractionService(IHttpClientWrapper httpClientWrapper)
    {
        _httpClientWrapper = httpClientWrapper;
    }
    public async Task<UserIdeaInteraction?> GetUserIdeaInteractionAsync(
        string username, int ideaId)
    {
        try
        {
            var response = await _httpClientWrapper.GetAsync(
                $"api/UserIdeaInteractions/{username}/{ideaId}");
            response.EnsureSuccessStatusCode(); // important, otherwise it will use std. model
            return await response.Content.ReadFromJsonAsync<UserIdeaInteraction>();
        }
        catch (Exception ex)
        {
            // All types of exceptions will land here, e.g.
            // timeout, no such user, server overload, ...
            // not sure if this is expected behaviour. (TODO)
            //return await Task.FromException<UserIdeaInteraction>(ex)
            //    .ConfigureAwait(false);
            Console.Write(ex.Message);
            return null;
        }
    }

    public async Task<bool> PutUserIdeaInteractionAsync(string username, int ideaId, bool newUpvoted, bool newDownvoted)
    {
        try
        {
            var httpContent = JsonContent.Create(new UserIdeaInteraction
            {
                Username = username,
                IdeaId = ideaId,
                Downvoted = newDownvoted,
                Upvoted = newUpvoted
            });
            var response = await _httpClientWrapper.PutAsync(
                $"api/UserIdeaInteractions/{username}/{ideaId}",
                httpContent);
            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return false;
        }
    }
}
