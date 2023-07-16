using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.MauiModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.ViewModel;

public partial class FeedEntryVM : ObservableObject
{
    private readonly IUserIdeaInteractionService _uiiService;
    private readonly IIdeasService _ideasService;

    public User CurrentUser { get; set; }
    [ObservableProperty]
    FeedEntry feedEntry;

    public FeedEntryVM(
        IUserIdeaInteractionService uiiService,
        IIdeasService ideasService)
    {
        _uiiService = uiiService;
        _ideasService = ideasService;
    }

    [RelayCommand]
    public async Task ChangeVote(string updown)
    {
        if (updown == "up")
        {
            if (FeedEntry.Upvoted == true)
            {
                if (await DbUpdateUiiAsync(false, FeedEntry.Downvoted))
                {
                    if (await DbUpdateIdeaVotesAsync(-1))
                    {
                        FeedEntry.Upvoted = false;
                        FeedEntry.Idea.Upvotes--;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("DB problem", "Uii updated, but Idea Votes not!!!!", "Bad");
                    }
                }
            }
            else
            {
                if (await DbUpdateUiiAsync(true, FeedEntry.Downvoted))
                {
                    if (await DbUpdateIdeaVotesAsync(+1))
                    {
                        FeedEntry.Upvoted = true;
                        FeedEntry.Idea.Upvotes++;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("DB problem", "Uii updated, but Idea Votes not!!!!", "Bad");
                    }
                }
            }
        }
        else if (updown == "down")
        {
            if (FeedEntry.Downvoted == true)
            {
                if (await DbUpdateUiiAsync(FeedEntry.Upvoted, false))
                {
                    if (await DbUpdateIdeaVotesAsync(+1))
                    {
                        FeedEntry.Downvoted = false;
                        FeedEntry.Idea.Upvotes++;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("DB problem", "Uii updated, but Idea Votes not!!!!", "Bad");
                    }
                }
            }
            else
            {
                if (await DbUpdateUiiAsync(FeedEntry.Upvoted, true))
                {
                    if (await DbUpdateIdeaVotesAsync(-1))
                    {
                        FeedEntry.Downvoted = true;
                        FeedEntry.Idea.Upvotes--;
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("DB problem", "Uii updated, but Idea Votes not!!!!", "Bad");
                    }
                }
            }
        }
    }
    private async Task<bool> DbUpdateUiiAsync(bool newUpvoted, bool newDownvoted)
    {
        try
        {
            return await _uiiService.PostOrPutUserIdeaInteractionAsync(
                new UserIdeaInteraction()
                {
                    Username = CurrentUser.Username,
                    IdeaId = FeedEntry.Idea.Id,
                    Upvoted = newUpvoted,
                    Downvoted = newDownvoted
                });
        }
        catch
        {
            await Shell.Current.DisplayAlert("DB Access Error", "Error while updating the UII (in the DB).", "Ok");
            return false;
        }
    }
    private async Task<bool> DbUpdateIdeaVotesAsync(int updown)
    {
        try
        {
            return await _ideasService.VoteIdeaAsync(FeedEntry.Idea.Id, updown);
        }
        catch
        {
            await Shell.Current.DisplayAlert("DB Access Error", "Error while updating the vote count (in the DB).", "Ok");
            return false;
        }
    }
}
