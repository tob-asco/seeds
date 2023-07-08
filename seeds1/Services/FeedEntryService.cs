using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;

namespace seeds1.Services;

public class FeedEntryService : IFeedEntryService
{
    private readonly IIdeasService _ideaService;
    private readonly ICategoryService _categoryService;
    public User CurrentUser { get; set; }
    public FeedEntryService(IIdeasService ideasService,
        ICategoryService categoryService)
    {
        _ideaService = ideasService;
        _categoryService = categoryService;
    }
    public async Task<List<FeedEntry>> GetFeedEntriesPaginated(int page, int maxPageSize)
    {
        List<FeedEntry> feedEntryPage = new();
        var ideaPage = await _ideaService.GetIdeasPaginated(page, maxPageSize);
        foreach (var idea in ideaPage)
        {
            var category = await _categoryService.GetCategoryByKey(idea.CategoryKey);
            var categoryPreference = ...
            feedEntryPage.Add(new FeedEntry
            {
                Idea = idea,
                CategoryName = category.Name,
                CategoryPreference = CurrentUser.CategoryUserPreferences.First(cup =>
                    cup.CategoryKey == idea.CategoryKey).Value
            });
        }
        return feedEntryPage;
    }
}
