using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.MauiModels;

public class FeedEntry
{
    public Idea Idea { get; set; }
    public string CategoryName { get; set; }
    public int CategoryPreference { get; set; }
    //public bool IdeaUpvoted { get; set; }
    //public bool IdeaDownvoted { get; set; }
    //public bool IdeaMarked { get; set; } //for later review
}
