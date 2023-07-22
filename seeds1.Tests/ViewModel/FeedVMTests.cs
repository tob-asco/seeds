using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.ViewModel;
using System.ComponentModel;

namespace seeds1.Tests.ViewModel;

public class FeedVMTests
{
    private readonly IFeedEntriesService _feedEntriesService;
    private readonly IUserIdeaInteractionService _uiiService;
    private readonly IIdeasService _ideasService;
    private readonly ICategoryUserPreferenceService _cupService;
    private readonly FeedViewModel _vm;
    public FeedVMTests()
    {
        _feedEntriesService = A.Fake<IFeedEntriesService>();
        _uiiService = A.Fake<IUserIdeaInteractionService>();
        _ideasService = A.Fake<IIdeasService>();
        _cupService = A.Fake<ICategoryUserPreferenceService>();
        _vm = new FeedViewModel(_feedEntriesService, _uiiService, _ideasService, _cupService);
    }

    [Fact]
    public async Task FeedVM_CollectFeedEntriesPaginated_AddsEntries()
    {
        // Arrange
        _vm.FeedEntryVMCollection = new(); //done in code-behind
        List<FeedEntry> feedEntries = new()
        {
            new FeedEntry {},
            new FeedEntry {}
        };
        A.CallTo(() => _feedEntriesService.GetFeedEntriesPaginated(
            A<int>.Ignored, A<int>.Ignored))
            .Returns(feedEntries);

        // Act
        await _vm.CollectFeedEntriesPaginated();

        // Assert
        _vm.FeedEntryVMCollection.Should().HaveCount(2);
    }


    [Fact]
    public async Task FeedVM_ChangeCatPreference_RaisePCEForCatPreference()
    {
        #region Arrange
        string key = "ABC";
        _vm.FeedEntryVMCollection = new()
        {
            new FeedEntryVM(_uiiService, _ideasService)
            {
                FeedEntry = new FeedEntry()
                {
                    CategoryName = "ABeCe",
                    CategoryPreference = -1,
                    Idea = new()
                    {
                        CategoryKey = key
                    }
                }
            },
            new FeedEntryVM(_uiiService, _ideasService)
            {
                FeedEntry = new FeedEntry()
                {
                   CategoryName = "ABeCe",
                   CategoryPreference = 1,
                   Idea = new()
                   {
                       CategoryKey = key
                   }
                }
            }
        };
        // the following is from:
        //https://github.com/CommunityToolkit/dotnet/blob/main/tests/CommunityToolkit.Mvvm.UnitTests/Test_INotifyPropertyChangedAttribute.cs
        List<PropertyChangedEventArgs> eventArgs = new();
        _vm.FeedEntryVMCollection[0].FeedEntry.PropertyChanged += (s, e) => eventArgs.Add(e);
        _vm.CurrentUser = new();
        A.CallTo(() => _cupService.PutCategoryUserPreferenceAsync(
            A<string>.Ignored, A<string>.Ignored, A<int>.Ignored))
            .Returns(true);
        #endregion

        // Act
        await _vm.ChangeCategoryPreference(key);

        // Assert
        eventArgs.Count.Should().BeGreaterThan(0);
        eventArgs[0].PropertyName.Should().NotBeNull();
        eventArgs[0].PropertyName?.ToLower().Should().Contain("pref");
    }
}
