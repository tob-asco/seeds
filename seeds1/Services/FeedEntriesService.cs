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
    private readonly IUserIdeaInteractionService uiiService;
    private readonly ICatagPreferencesService catagPreferencesService;

    public FeedEntriesService(
        IGlobalService globalService,
        IIdeasService ideasService,
        IUserIdeaInteractionService uiiService,
        ICatagPreferencesService catagPreferencesService)
    {
        this.globalService = globalService;
        this.ideasService = ideasService;
        this.uiiService = uiiService;
        this.catagPreferencesService = catagPreferencesService;
    }
    public async Task<List<UserFeedentry>> GetFeedEntriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        List<UserFeedentry> feedEntryPage = new();
        var ideaPage = await ideasService.GetIdeasPaginatedAsync(
            pageIndex, pageSize, orderByColumn, isDescending);
        if (ideaPage == null) { return new(); } // we get null if there are no more ideas
        foreach (var idea in ideaPage)
        {
            /* According to the general philo, no error- / badNull- handling here.
             * badNull-handling is done in the DAL services,
             * error-handling is done in the VMs
             */
            var upvotes = await uiiService.CountVotesAsync(idea.Id);
            //var tagPrefs = await catagPreferencesService.GetTagPreferencesOfIdeaAsync(idea);
            var uii = globalService.GetIdeaInteractions()[idea.Id]
                ?? new UserIdeaInteraction();
            feedEntryPage.Add(new UserFeedentry
            {
                Idea = idea,
                //CatagPreferences = tagPrefs,
                Upvoted = uii.Upvoted,
                Downvoted = uii.Downvoted,
                Upvotes = upvotes,
            });
        }
        return feedEntryPage;
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
