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
    public Idea Idea { get; set; }
    public string CategoryName { get; set; }
    [ObservableProperty]
    int categoryPreference;
    [ObservableProperty]
    bool upvoted;
    [ObservableProperty]
    bool downvoted;
    //public bool IdeaMarked { get; set; } //for later review
}
