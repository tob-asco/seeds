using seeds1.Interfaces;
using seeds1.MauiModels;

namespace seeds1.ViewModel;

[QueryProperty(nameof(FeedEntry), nameof(FeedEntry))]
public partial class DetailViewModel : MyBaseViewModel
{
    public FeedEntry FeedEntry { get; set; }
    public DetailViewModel(
        IGlobalService globalService)
        : base(globalService)
    {

    }
}
