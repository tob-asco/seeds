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
        if (feedentryPage == null) { return new(); } // we get null if there are no more ideas
        foreach (var fe in feedentryPage)
        {
            /* According to the general philo, no error- / badNull- handling here.
             * badNull-handling is done in the DAL services,
             * error-handling is done in the VMs
             */
            List<CatagPreference> tagPrefs = new();
            foreach(var tag in fe.Tags)
            {
                tagPrefs.Add(new()
                {
                    CategoryKey = tag.CategoryKey,
                    // TODO: CategoryName
                    TagName = tag.Name,
                    Preference = globalService.GetPreferences()[tag.Id].Value
                });
            }
            userFePage.Add(new UserFeedentry
            {
                Idea = fe.Idea,
                CatagPreferences = tagPrefs,
                Upvoted = globalService.GetIdeaInteractions()[fe.Idea.Id].Upvoted,
                Downvoted = globalService.GetIdeaInteractions()[fe.Idea.Id].Downvoted,
                Upvotes = fe.Upvotes,
            });
        }
        return userFePage;
    }
}
