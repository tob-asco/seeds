using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class FeedEntriesService : IFeedEntriesService
{
    private readonly IGlobalService globalService;
    private readonly IIdeasService ideasService;

    public FeedEntriesService(
        IGlobalService globalService,
        IIdeasService ideasService)
    {
        this.globalService = globalService;
        this.ideasService = ideasService;
    }
    public async Task<List<UserFeedentry>> GetUserFeedentriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        List<UserFeedentry> userFePage = new();
        var feedentryPage = await ideasService.GetFeedentriesPaginatedAsync(
            pageIndex, pageSize, orderByColumn, isDescending);
        if (feedentryPage.Count == 0) { return new(); } // we get null if there are no more ideas
        foreach (var fe in feedentryPage)
        {
            /* According to the general philo, no error- / badNull- handling here.
             * badNull-handling is done in the DAL services,
             * error-handling is done in the VMs
             */
            List<MauiPreference> topicPrefs = new();
            foreach(var topic in fe.Topics)
            {
                topicPrefs.Add(new()
                {
                    Topic = topic,
                    Preference = globalService.GetPreferences().ContainsKey(topic.Id) ?
                        globalService.GetPreferences()[topic.Id].Value : 0
                });
            }
            userFePage.Add(new UserFeedentry
            {
                Idea = fe.Idea,
                MauiPreferences = topicPrefs,
                Upvoted = globalService.GetIdeaInteractions().ContainsKey(fe.Idea.Id) ?
                    globalService.GetIdeaInteractions()[fe.Idea.Id].Upvoted : false,
                Downvoted = globalService.GetIdeaInteractions().ContainsKey(fe.Idea.Id) ?
                    globalService.GetIdeaInteractions()[fe.Idea.Id].Upvoted : false,
                Upvotes = fe.Upvotes,
            });
        }
        return userFePage;
    }
}
