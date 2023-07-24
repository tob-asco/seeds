using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.ViewModel;

public partial class FeedEntryViewModel : ObservableObject
{
    private readonly IGlobalService globalService;
    private readonly IUserIdeaInteractionService uiiService;

    public FeedEntry FeedEntry { get; set; }

    public FeedEntryViewModel(
        IGlobalService globalService,
        IUserIdeaInteractionService uiiService)
    {
        this.globalService = globalService;
        this.uiiService = uiiService;
    }

    [RelayCommand]
    public async Task ChangeVote(string updown)
    {
        try
        {
            if (updown == "up")
            {
                if (FeedEntry.Upvoted == true)
                {
                    await DbUpdateUiiAsync(false, FeedEntry.Downvoted);
                    FeedEntry.Upvoted = false;
                    FeedEntry.Upvotes--;
                }
                else
                {
                    await DbUpdateUiiAsync(true, FeedEntry.Downvoted);
                    FeedEntry.Upvoted = true;
                    FeedEntry.Upvotes++;
                }
            }
            else if (updown == "down")
            {
                if (FeedEntry.Downvoted == true)
                {
                    await DbUpdateUiiAsync(FeedEntry.Upvoted, false);
                    FeedEntry.Downvoted = false;
                    FeedEntry.Upvotes++;
                }
                else
                {
                    await DbUpdateUiiAsync(FeedEntry.Upvoted, true);
                    FeedEntry.Downvoted = true;
                    FeedEntry.Upvotes--;
                }
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Access Error", ex.Message, "Ok");
        }
    }
    private async Task DbUpdateUiiAsync(bool newUpvoted, bool newDownvoted)
    {
        await uiiService.PostOrPutUserIdeaInteractionAsync(
            new UserIdeaInteraction()
            {
                Username = globalService.CurrentUser.Username,
                IdeaId = FeedEntry.Idea.Id,
                Upvoted = newUpvoted,
                Downvoted = newDownvoted
            });
    }
}
