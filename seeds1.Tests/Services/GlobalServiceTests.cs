using Microsoft.Maui.Platform;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Factories;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;
using seeds1.View;
using seeds1.ViewModel;

namespace seeds1.Tests.Services;

public class GlobalServiceTests
{
    private readonly IStaticService stat;
    private readonly IIdeasService ideasService;
    private readonly IGenericFactory<FeedEntryViewModel> feedEntryVmFactory;
    private readonly IUserPreferenceService userPrefService;
    private readonly IUserIdeaInteractionService uiiService;
    private readonly GlobalService service;
    public GlobalServiceTests()
    {
        stat = A.Fake<IStaticService>();
        ideasService = A.Fake<IIdeasService>();
        feedEntryVmFactory = A.Fake<IGenericFactory<FeedEntryViewModel> >();
        userPrefService = A.Fake<IUserPreferenceService>();
        uiiService = A.Fake<IUserIdeaInteractionService>();
        service = new(stat, ideasService, feedEntryVmFactory, userPrefService, uiiService);
        service.CurrentUser = new();
    }

    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatesUPsDict()
    {
        // Arrange
        List<UserPreference> ups = new()
        {
            new(){ItemId = Guid.NewGuid(), Username = "ich"},
            new(){ItemId = Guid.NewGuid(), Username = "ich"},
            new(){ItemId = Guid.NewGuid(), Username = "du"},
        };
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns(ups);
        A.CallTo(() => userPrefService.GetButtonedTopicsOfUserAsync(A<string>.Ignored))
            .Returns<List<TopicFromDb>>(new());

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        service.GetPreferences().Should().HaveCount(ups.Count);
        service.GetPreferences().Should().ContainKey(ups[0].ItemId);
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatesFOPsWithTopics()
    {
        // Arrange
        List<TopicFromDb> topics = new()
        {
            new(){Id = Guid.NewGuid(), Name = "ich"},
            new(){Id = Guid.NewGuid(), Name = "ich"},
            new(){Id = Guid.NewGuid(), Name = "du"},
        };
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(new());
        A.CallTo(() => userPrefService.GetButtonedTopicsOfUserAsync(A<string>.Ignored))
            .Returns<List<TopicFromDb>>(topics);
        A.CallTo(() => stat.GetCategories())
            .Returns(new Dictionary<string, CategoryDto> { { "NoC", new() } });

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        fopList.Should().HaveCount(topics.Count);
        foreach (var topic in topics)
        {
            fopList.Should().Contain(t => t.Preference.Topic.Id == topic.Id);
        }
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatedTopicsHaveCorrectPreference()
    {
        // Arrange
        Guid topicId = Guid.NewGuid();
        int val = 1;
        List<TopicFromDb> topics = new() { new() { Id = topicId, Name = "du" }, };
        List<UserPreference> ups = new() { new() { ItemId = topicId, Value = val } };
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(ups);
        A.CallTo(() => userPrefService.GetButtonedTopicsOfUserAsync(A<string>.Ignored))
            .Returns<List<TopicFromDb>>(topics);
        A.CallTo(() => stat.GetCategories())
            .Returns(new Dictionary<string, CategoryDto> { { "NoC", new() } });

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        fopList.Should().HaveCount(topics.Count);
        foreach (var topic in topics)
        {
            fopList.Should().Contain(t =>
                t.Preference.Topic.Id == topicId && t.Preference.Preference == val);
        }
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatesFOPsWithFams()
    {
        // Arrange
        Dictionary<Guid, FamilyFromDb> fams = new()
        {
            {Guid.NewGuid(), new() { Name = "wir" } },
            {Guid.NewGuid(), new() { Name = "ihr" } },
        };
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(new());
        A.CallTo(() => userPrefService.GetButtonedTopicsOfUserAsync(A<string>.Ignored))
            .Returns<List<TopicFromDb>>(new());
        A.CallTo(() => stat.GetFamilies())
            .Returns(fams);
        A.CallTo(() => stat.GetCategories())
            .Returns(new Dictionary<string, CategoryDto> { { "NoC", new() } });

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        fopList.Should().HaveCount(fams.Count);
        foreach (var fam in fams)
        {
            fopList.Should().Contain(t => t.Family.Id == fam.Value.Id);
        }
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_IfUPsAlreadyLoadedNothingChanges()
    {
        // Arrange for first time
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(new());
        A.CallTo(() => userPrefService.GetButtonedTopicsOfUserAsync(A<string>.Ignored))
            .Returns<List<TopicFromDb>>(new());
        // Act first time
        await service.LoadPreferencesAsync();

        // Arrange for second time
        string s = "wir";
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(new() { new() { Username = s } });
        A.CallTo(() => userPrefService.GetButtonedTopicsOfUserAsync(A<string>.Ignored))
            .Returns<List<TopicFromDb>>(new() { new() { Name = s } });
        // Act second time
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        service.GetPreferences().Values.Should().NotContain(up => up.Username == s);
        fopList.Should().NotContain(fop => fop.Preference.Topic.Name == s);
    }
    [Fact]
    public async Task GlobalService_GlobChangePreferenceAsync_ChangesPrefOfOrphanTopicInFOPsAndPrefs()
    {
        // Arrange
        string key = "cat";
        Guid id = Guid.NewGuid();
        int pref = 0;
        Dictionary<Guid, TopicFromDb> topics = new() { { id, new() { Id = id, CategoryKey = key } } };
        service.FopListDict.Add(key, new() { new() {
            CategoryKey = key,
            IsFamily = false,
            Preference = new MauiPreference(){
                Topic=topics[id],
                Preference=pref
            }}
        });
        A.CallTo(() => stat.GetTopics()).Returns(topics);
        service.PreferencesLoaded = true;

        // Act
        int newPref = 1;
        await service.GlobChangePreferenceAsync(id, newPref);

        // Assert
        service.FopListDict[key].Should().Contain(fop =>
            fop.Preference.Topic.Id == id && fop.Preference.Preference == newPref);
        service.GetPreferences()[id].Value.Should().Be(newPref);
    }
    [Fact]
    public async Task GlobalService_GlobChangePreferenceAsync_AddsFOPForUnbuttonedTopicWithFamilyReturnsFalse()
    {
        #region Arrange
        string key = "cat";
        Guid topicId = Guid.NewGuid();
        Guid famId = Guid.NewGuid();
        // add a topic that is in a family
        Dictionary<Guid, TopicFromDb> topics = new() { { topicId, new() {
            Id = topicId, CategoryKey = key, FamilyId = famId
        } } };
        // add the corresponding family to FopListDict
        service.FopListDict.Add(key, new() { new() {
            CategoryKey = key,
            IsFamily = true,
            Family = new(){
                CategoryKey = key, Id = famId, Topics = topics.Values.ToList()
            }}
        });
        A.CallTo(() => stat.GetTopics()).Returns(topics);
        #endregion

        // Act
        int newPref = 1;
        bool topicAlreadyButtoned = await service.GlobChangePreferenceAsync(topicId, newPref);

        // Assert
        service.FopListDict[key].Should().Contain(fop =>
            !fop.IsFamily && fop.Preference.Topic.Id == topicId && fop.Preference.Preference == newPref);
        topicAlreadyButtoned.Should().BeFalse();
    }
    [Fact]
    public async Task GlobalService_GlobChangePreferenceAsync_UpdatesFOPForButtonedTopicWithFamilyReturnsTrue()
    {
        #region Arrange
        string key = "cat";
        Guid topicId = Guid.NewGuid();
        Guid famId = Guid.NewGuid();
        int pref = -1;
        // add a topic that is in a family
        Dictionary<Guid, TopicFromDb> topics = new() { { topicId, new() {
            Id = topicId, CategoryKey = key, FamilyId = famId
        } } };
        // add the corresponding family to FopListDict
        service.FopListDict.Add(key, new() { new() {
            CategoryKey = key,
            IsFamily = true,
            Family = new(){
                CategoryKey = key, Id = famId, Topics = topics.Values.ToList()
            }}
        });
        // add a preference for this topic
        service.FopListDict[key].Add(new()
        {
            CategoryKey = key,
            IsFamily = false,
            Preference = new MauiPreference()
            {
                Topic = topics[topicId],
                Preference = pref
            }
        });
        A.CallTo(() => stat.GetTopics()).Returns(topics);
        #endregion

        // Act
        int newPref = 1;
        bool topicAlreadyButtoned = await service.GlobChangePreferenceAsync(topicId, newPref);

        // Assert
        service.FopListDict[key].Should().Contain(fop =>
            !fop.IsFamily && fop.Preference.Topic.Id == topicId && fop.Preference.Preference == newPref);
        topicAlreadyButtoned.Should().BeTrue();
    }
}
