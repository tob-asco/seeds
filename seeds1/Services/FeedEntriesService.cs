﻿using seeds.Dal.Model;
using seeds.Dal.Services;
using seeds1.MauiModels;

namespace seeds1.Services;

public class FeedEntriesService : IFeedEntriesService
{
    private readonly IIdeasService _ideaService;
    private readonly ICategoryService _categoryService;
    private readonly ICategoryUserPreferenceService _cupService;
    private readonly IUserIdeaInteractionService _uiiService;
    public User CurrentUser { get; set; }
    public FeedEntriesService(IIdeasService ideasService,
        ICategoryService categoryService,
        ICategoryUserPreferenceService categoryUserPreferenceService)
    {
        _ideaService = ideasService;
        _categoryService = categoryService;
        _cupService = categoryUserPreferenceService;
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
                var cup = await _cupService.GetCategoryUserPreferenceAsync(
                    idea.CategoryKey, CurrentUser.Username);
                var uii = await _uiiService.GetUserIdeaInteractionAsync(
                    CurrentUser.Username, idea.Id);
                feedEntryPage.Add(new FeedEntry
                {
                    Idea = idea,
                    CategoryName = category.Name,
                    CategoryPreference = cup.Value,
                    Upvoted = uii.Upvoted,
                    Downvoted = uii.Downvoted,
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