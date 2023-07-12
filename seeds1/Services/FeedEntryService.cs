using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;

namespace seeds1.Services;

public class FeedEntryService : IFeedEntryService
{
    private readonly IIdeasService _ideaService;
    private readonly ICategoryService _categoryService;
    private readonly ICategoryUserPreferenceService _categoryUserPreferenceService;
    public User CurrentUser { get; set; }
    public FeedEntryService(IIdeasService ideasService,
        ICategoryService categoryService,
        ICategoryUserPreferenceService categoryUserPreferenceService)
    {
        _ideaService = ideasService;
        _categoryService = categoryService;
        _categoryUserPreferenceService = categoryUserPreferenceService;
    }
    public async Task<List<FeedEntry>> GetFeedEntriesPaginated(int page, int maxPageSize)
    {
        List<FeedEntry> feedEntryPage = new();
        var ideaPage = await _ideaService.GetIdeasPaginatedAsync(page, maxPageSize);
        foreach (var idea in ideaPage)
        {
            try
            {
                var category = await _categoryService.GetCategoryByKeyAsync(idea.CategoryKey);
                var categoryPreference = await _categoryUserPreferenceService.GetCategoryUserPreferenceAsync(
                    idea.CategoryKey, CurrentUser.Username);
                feedEntryPage.Add(new FeedEntry
                {
                    Idea = idea,
                    CategoryName = category.Name,
                    CategoryPreference = categoryPreference.Value
                });
            }
            catch (Exception ex)
            {
                // we will not have this feedEntry
                Console.WriteLine(ex);
            }
        }
        return feedEntryPage;
    }
}
