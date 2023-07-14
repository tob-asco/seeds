using seeds.Dal.Model;
using seeds.Dal.Services;
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
    public async Task ChangeVote(int updown)
    {
        int oldUpvotes = FeedEntry.Idea.Upvotes;
        if (updown == +1)
        {
            if (FeedEntry.Upvoted == true)
            {
                FeedEntry.Upvoted = false;
                FeedEntry.Idea.Upvotes--;
            }
            else
            {
                FeedEntry.Upvoted = true;
                FeedEntry.Idea.Upvotes++;
            }
        }
        else if (updown == -1)
        {
            if (FeedEntry.Downvoted == true)
            {
                FeedEntry.Downvoted = false;
                FeedEntry.Idea.Upvotes++;
            }
            else
            {
                FeedEntry.Downvoted = true;
                FeedEntry.Idea.Upvotes--;
            }
        }

        //DB
        try
        {
            var success1 = await _uiiService.PutUserIdeaInteractionAsync(
                CurrentUser.Username,
                FeedEntry.Idea.Id,
                FeedEntry.Upvoted,
                FeedEntry.Downvoted);
            var success2 = await _ideasService.VoteIdeaAsync(
                FeedEntry.Idea.Id,
                FeedEntry.Idea.Upvotes - oldUpvotes);
            if (success1 == false || success2 == false) { throw new Exception(); }
        }
        catch
        {
            await Shell.Current.DisplayAlert("DB Access Error", "The DB could not be accessed.\n" +
                "Refresh to see the actual state.", "Ok");
        }
    }
}
