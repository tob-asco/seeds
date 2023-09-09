using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.ViewModel;
using System.ComponentModel;

namespace seeds1.Tests.ViewModel;

public class FeedVMTests
{
    private readonly IStaticService staticService;
    private readonly IGlobalService globalService;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IFeedEntriesService feedEntriesService;
    private readonly IUserPreferenceService prefService;
    private readonly IUserIdeaInteractionService uiiService;
    private readonly FeedViewModel _vm;
    public FeedVMTests()
    {
        staticService = A.Fake<IStaticService>();
        globalService = A.Fake<IGlobalService>();
        feedEntryVmFactory = A.Fake<IGenericFactory<FeedEntryViewModel>>();
        feedEntriesService = A.Fake<IFeedEntriesService>();
        prefService = A.Fake<IUserPreferenceService>();
        uiiService = A.Fake<IUserIdeaInteractionService>();
        _vm = new FeedViewModel(staticService, globalService, feedEntryVmFactory, prefService, feedEntriesService);
    }

    //[Fact]
    //public async Task FeedVM_CollectFeedEntriesPaginated_AddsEntries()
    //{
        //// arrange
        //_vm.feedentryvmcollection = new(); //done in code-behind
        //list<userfeedentry> feedentries = new()
        //{
        //    new userfeedentry {},
        //    new userfeedentry {}
        //};
        //a.callto(() => feedentriesservice.getuserfeedentriespaginatedasync(
        //    a<int>.ignored, a<int>.ignored, a<string>.ignored, a<bool>.ignored))
        //    .returns(feedentries);

        //// act
        //await _vm.collectfeedentriespaginated();

        //// assert
        //_vm.feedentryvmcollection.should().havecount(2);
    //}

    /*
    [Fact]
    public async Task FeedVM_ChangeCatPreference_RaisePCEForCatPreference()
    {
        #region Arrange
        string key = "ABC";
        _vm.FeedEntryVMCollection = new()
        {
            new FeedEntryViewModel(globalService, uiiService)
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
            new FeedEntryViewModel(globalService, uiiService)
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
        A.CallTo(() => cupService.PutMauiUserPreferenceAsync(
            A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<string?>.Ignored))
            .Returns(true);
        #endregion

        // Act
        await _vm.ChangeCategoryPreference(key);

        // Assert
        eventArgs.Count.Should().BeGreaterThan(0);
        eventArgs[0].PropertyName.Should().NotBeNull();
        eventArgs[0].PropertyName?.ToLower().Should().Contain("pref");
    }
    */
}
