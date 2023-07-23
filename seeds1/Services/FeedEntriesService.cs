using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.Services;

public class FeedEntriesService : IFeedEntriesService
{
    private readonly IGlobalService globalService;
    private readonly IIdeasService ideasService;
    private readonly ICategoryService categoryService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly IUserIdeaInteractionService uiiService;
    public FeedEntriesService(
        IGlobalService globalService,
        IIdeasService ideasService,
        ICategoryService categoryService,
        ICategoryUserPreferenceService cupService,
        IUserIdeaInteractionService uiiService)
    {
        this.globalService = globalService;
        this.ideasService = ideasService;
        this.categoryService = categoryService;
        this.cupService = cupService;
        this.uiiService = uiiService;
    }
    public async Task<List<FeedEntry>> GetFeedEntriesPaginatedAsync(int page, int maxPageSize)
    {
        List<FeedEntry> feedEntryPage = new();
        var ideaPage = await ideasService.GetIdeasPaginatedAsync(page, maxPageSize);
        // we get null if there are no more ideas
        if (ideaPage == null) { return new(); }
        foreach (var idea in ideaPage)
        {
            try
            {
                var upvotes = await uiiService.CountVotesAsync(idea.Id);
                var category = await categoryService.GetCategoryByKeyAsync(idea.CategoryKey)
                    ?? throw new Exception($"Category with key {idea.CategoryKey} returned null.");
                var cup = await cupService.GetCategoryUserPreferenceAsync(
                    idea.CategoryKey, globalService.CurrentUser.Username)
                    ?? throw new Exception($"Category with key {idea.CategoryKey} returned" +
                    $" user preference null.");
                var uii = await uiiService.GetUserIdeaInteractionAsync(
                    globalService.CurrentUser.Username, idea.Id)
                    ?? new UserIdeaInteraction();
                feedEntryPage.Add(new FeedEntry
                {
                    Idea = idea,
                    CategoryName = category.Name,
                    CategoryPreference = cup.Value,
                    Upvoted = uii.Upvoted,
                    Downvoted = uii.Downvoted,
                    Upvotes = upvotes,
                });
            }
            catch (Exception ex)
            {
                // we will not have this FeedEntry
                Console.WriteLine(ex);
            }
        }
        return feedEntryPage;
    }
}
