using seeds.Dal.Dto.ToApi;
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

    public UserDtoApi CurrentUser { get; set; }
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
                    FeedEntry.Upvoted = false;
                    FeedEntry.Upvotes--;
                }
            }
            else
            {
                if (await DbUpdateUiiAsync(true, FeedEntry.Downvoted))
                {
                    FeedEntry.Upvoted = true;
                    FeedEntry.Upvotes++;
                }
            }
        }
        else if (updown == "down")
        {
            if (FeedEntry.Downvoted == true)
            {
                if (await DbUpdateUiiAsync(FeedEntry.Upvoted, false))
                {
                    FeedEntry.Downvoted = false;
                    FeedEntry.Upvotes++;
                }
            }
            else
            {
                if (await DbUpdateUiiAsync(FeedEntry.Upvoted, true))
                {
                    FeedEntry.Downvoted = true;
                    FeedEntry.Upvotes--;
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
}
