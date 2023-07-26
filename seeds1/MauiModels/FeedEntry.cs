using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds1.MauiModels;

public partial class FeedEntry : ObservableObject
{
    public IdeaFromDb Idea { get; set; } = new();
    public string CategoryName { get; set; } = "No Category";
    [ObservableProperty]
    int categoryPreference = 0;
    [ObservableProperty]
    bool upvoted = false;
    [ObservableProperty]
    bool downvoted = false;
    [ObservableProperty]
    int upvotes = 0;
    //public bool IdeaMarked { get; set; } //for later review
}
