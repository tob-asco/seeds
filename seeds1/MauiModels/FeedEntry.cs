using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.MauiModels;

public partial class FeedEntry : ObservableObject
{
    public Idea Idea { get; set; } = new();
    public string CategoryName { get; set; } = "No Category";
    [ObservableProperty]
    int categoryPreference = 0;
    [ObservableProperty]
    bool upvoted = false;
    [ObservableProperty]
    bool downvoted = false;
    //public bool IdeaMarked { get; set; } //for later review
}
