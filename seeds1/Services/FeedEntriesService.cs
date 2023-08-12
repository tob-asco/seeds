﻿using seeds.Dal.Dto.FromDb;
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
    private readonly ICatagUserPreferenceService cupService;
    private readonly IUserIdeaInteractionService uiiService;
    public FeedEntriesService(
        IGlobalService globalService,
        IIdeasService ideasService,
        ICategoryService categoryService,
        ICatagUserPreferenceService cupService,
        IUserIdeaInteractionService uiiService)
    {
        this.globalService = globalService;
        this.ideasService = ideasService;
        this.categoryService = categoryService;
        this.cupService = cupService;
        this.uiiService = uiiService;
    }
    public async Task<List<FeedEntry>> GetFeedEntriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true)
    {
        List<FeedEntry> feedEntryPage = new();
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
            var category = await categoryService.GetCategoryByKeyAsync(idea.CategoryKey);
            var cup = await cupService.GetCatagUserPreferenceAsync(
                idea.CategoryKey, globalService.CurrentUser.Username);
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
        return feedEntryPage;
    }
}
