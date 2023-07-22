using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.ViewModel;

public partial class FeedEntryVM : ObservableObject
{
    private readonly IGlobalVmService globalService;
    private readonly IUserIdeaInteractionService uiiService;

    public FeedEntry FeedEntry { get; set; }

    public FeedEntryVM(
        IGlobalVmService globalService,
        IUserIdeaInteractionService uiiService)
    {
        this.globalService = globalService;
        this.uiiService = uiiService;
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
            return await uiiService.PostOrPutUserIdeaInteractionAsync(
                new UserIdeaInteraction()
                {
                    Username = globalService.CurrentUser.Username,
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
