using seeds.Dal.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;
using seeds1.ViewModel;
using System.ComponentModel;

namespace seeds1.Tests.ViewModel;

public class FeedVMTests
{
    private readonly IFeedEntryService _feedEntryService;
    private readonly ICategoryUserPreferenceService _cupService;
    private readonly FeedViewModel _vm;
    public FeedVMTests()
    {
        _feedEntryService = A.Fake<IFeedEntryService>();
        _cupService = A.Fake<ICategoryUserPreferenceService>();
        _vm = new FeedViewModel(_feedEntryService, _cupService);
    }

    [Fact]
    public async Task FeedVM_CollectFeedEntriesPaginated_AddsEntries()
    {
        // Arrange
        _vm.FeedEntryCollection = new(); //done in code-behind
        List<FeedEntry> feedEntries = new()
        {
            new FeedEntry {},
            new FeedEntry {}
        };
        A.CallTo(() => _feedEntryService.GetFeedEntriesPaginated(
            A<int>.Ignored, A<int>.Ignored))
            .Returns(feedEntries);

        // Act
        await _vm.CollectFeedEntriesPaginated();

        // Assert
        _vm.FeedEntryCollection.Should().HaveCount(2);
    }


    [Fact]
    public async Task FeedVM_ChangeCatPreference_RaisePCEForCatPreference()
    {
        #region Arrange
        string key = "ABC";
        _vm.FeedEntryCollection = new()
        {
            new FeedEntry()
            {
                CategoryName = "ABeCe",
                CategoryPreference = -1,
                Idea = new seeds.Dal.Model.Idea()
                {
                    CategoryKey = key
                }
            },
            new FeedEntry()
            {
                CategoryName = "ABeCe",
                CategoryPreference = 1,
                Idea = new seeds.Dal.Model.Idea()
                {
                    CategoryKey = key
                }
            }
        };
        // the following is from:
        //https://github.com/CommunityToolkit/dotnet/blob/main/tests/CommunityToolkit.Mvvm.UnitTests/Test_INotifyPropertyChangedAttribute.cs
        List<PropertyChangedEventArgs> eventArgs = new();
        _vm.FeedEntryCollection[0].PropertyChanged += (s, e) => eventArgs.Add(e);
        _vm.CurrentUser = new();
        A.CallTo(() => _cupService.PutCategoryUserPreferenceAsync(
            A<string>.Ignored, A<string>.Ignored, A<int>.Ignored))
            .Returns(true);
        #endregion

        // Act
        await _vm.ChangeCategoryPreference(key);

        // Assert
        eventArgs.Count.Should().BeGreaterThan(0);
        eventArgs[0].PropertyName.ToLower().Should().Contain("pref");
    }
}
