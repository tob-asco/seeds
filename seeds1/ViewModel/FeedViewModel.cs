using MvvmHelpers;
using seeds.Dal.Model;
using seeds.Dal.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace seeds1.ViewModel;

//    ...     ( property here   ...   , queryId    ...      )]
[QueryProperty(nameof(CurrentUsername),nameof(User.Username))]
public partial class FeedViewModel : BasisViewModel
{
    private static readonly int _maxIdeaPageSize = 10;
    //private readonly User _currentUser;
    private readonly IIdeasService _ideaService;
    [ObservableProperty]
    ObservableRangeCollection<Idea> ideaCollection;
    public FeedViewModel(IIdeasService ideasService)
    {
        _ideaService = ideasService;
        IdeaCollection = new();
    }

    [RelayCommand]
    public async Task CollectIdeasPaginated()
    {
        int currentCount;
        if (IdeaCollection == null) { currentCount = 0; }
        else { currentCount = IdeaCollection.Count; }

        int currentPages = (int)Math.Ceiling((decimal)currentCount / _maxIdeaPageSize);
        try
        {
            var ideas = await _ideaService.GetIdeasPaginated(
                currentPages + 1, _maxIdeaPageSize);
            //ideas.Reverse();
            IdeaCollection.AddRange(ideas);
        }
        catch //(Exception ex) 
        {
            //not too bad, possibly just no more ideas
            return;
        }
    }
}
