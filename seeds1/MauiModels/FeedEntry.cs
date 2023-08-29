﻿using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds1.MauiModels;

public partial class FeedEntry : ObservableObject
{
    public IdeaFromDb Idea { get; set; } = new();
    public List<CatagPreference> CatagPreferences { get; set; } = new();
    [ObservableProperty]
    bool upvoted = false;
    [ObservableProperty]
    bool downvoted = false;
    [ObservableProperty]
    int upvotes = 0;
    //public bool IdeaMarked { get; set; } //for later review
}
