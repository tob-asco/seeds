using Microsoft.Maui.Platform;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
using seeds1.MauiModels;
using seeds1.Services;

namespace seeds1.Tests.Services;

public class GlobalServiceTests
{
    private readonly IStaticService stat;
    private readonly IUserPreferenceService userPrefService;
    private readonly IUserIdeaInteractionService uiiService;
    private readonly GlobalService service;
    public GlobalServiceTests()
    {
        stat = A.Fake<IStaticService>();
        userPrefService = A.Fake<IUserPreferenceService>();
        uiiService = A.Fake<IUserIdeaInteractionService>();
        service = new(stat, userPrefService, uiiService);
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
        A.CallTo(() => userPrefService.GetButtonedTagsOfUserAsync(A<string>.Ignored))
            .Returns<List<TagFromDb>>(new());

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        service.GetPreferences().Should().HaveCount(ups.Count);
        service.GetPreferences().Should().ContainKey(ups[0].ItemId);
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatesFOPsWithTags()
    {
        // Arrange
        List<TagFromDb> tags = new()
        {
            new(){Id = Guid.NewGuid(), Name = "ich"},
            new(){Id = Guid.NewGuid(), Name = "ich"},
            new(){Id = Guid.NewGuid(), Name = "du"},
        };
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(new());
        A.CallTo(() => userPrefService.GetButtonedTagsOfUserAsync(A<string>.Ignored))
            .Returns<List<TagFromDb>>(tags);

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        fopList.Should().HaveCount(tags.Count);
        foreach (var tag in tags)
        {
            fopList.Should().Contain(t => t.Preference.Tag.Id == tag.Id);
        }
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatedTagsHaveCorrectPreference()
    {
        // Arrange
        Guid tagId = Guid.NewGuid();
        int val = 1;
        List<TagFromDb> tags = new() { new(){Id = tagId, Name = "du"}, };
        List<UserPreference> ups = new() { new() { ItemId = tagId, Value = val } };
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(ups);
        A.CallTo(() => userPrefService.GetButtonedTagsOfUserAsync(A<string>.Ignored))
            .Returns<List<TagFromDb>>(tags);

        // Act
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        fopList.Should().HaveCount(tags.Count);
        foreach (var tag in tags)
        {
            fopList.Should().Contain(t =>
                t.Preference.Tag.Id == tagId && t.Preference.Preference == val);
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
        A.CallTo(() => userPrefService.GetButtonedTagsOfUserAsync(A<string>.Ignored))
            .Returns<List<TagFromDb>>(new());
        A.CallTo(() => stat.GetFamilies())
            .Returns(fams);

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
        A.CallTo(() => userPrefService.GetButtonedTagsOfUserAsync(A<string>.Ignored))
            .Returns<List<TagFromDb>>(new());
        // Act first time
        await service.LoadPreferencesAsync();

        // Arrange for second time
        string s = "wir";
        A.CallTo(() => userPrefService.GetPreferencesOfUserAsync(A<string>.Ignored))
            .Returns<List<UserPreference>>(new() { new() { Username = s } });
        A.CallTo(() => userPrefService.GetButtonedTagsOfUserAsync(A<string>.Ignored))
            .Returns<List<TagFromDb>>(new() { new() { Name = s } });
        // Act second time
        await service.LoadPreferencesAsync();

        // Assert
        var fopList = service.FopListList.SelectMany(l => l).ToList();
        service.GetPreferences().Values.Should().NotContain(up => up.Username == s);
        fopList.Should().NotContain(fop => fop.Preference.Tag.Name == s);
    }
    [Fact]
    public async Task GlobalService_GlobChangePreferenceAsync_ChangesPrefOfOrphanTagInFOPsAndPrefs()
    {
        // Arrange
        string key = "cat";
        Guid id = Guid.NewGuid();
        int pref = 0;
        Dictionary<Guid, TagFromDb> tags = new() { { id, new() { Id = id, CategoryKey = key } } };
        service.FopListDict.Add(key, new() { new() {
            CategoryKey = key,
            IsFamily = false,
            Preference = new CatagPreference(){
                Tag=tags[id],
                Preference=pref
            }}
        });
        A.CallTo(() => stat.GetTags()).Returns(tags);
        service.PreferencesLoaded = true;

        // Act
        int newPref = 1;
        await service.GlobChangePreferenceAsync(id, newPref);

        // Assert
        service.FopListDict[key].Should().Contain(fop =>
            fop.Preference.Tag.Id == id && fop.Preference.Preference == newPref);
        service.GetPreferences()[id].Value.Should().Be(newPref);
    }
    [Fact]
    public async Task GlobalService_GlobChangePreferenceAsync_AddsFOPForUnbuttonedTagWithFamilyReturnsFalse()
    {
        #region Arrange
        string key = "cat";
        Guid tagId = Guid.NewGuid();
        Guid famId = Guid.NewGuid();
        // add a tag that is in a family
        Dictionary<Guid, TagFromDb> tags = new() { { tagId, new() {
            Id = tagId, CategoryKey = key, FamilyId = famId
        } } };
        // add the corresponding family to FopListDict
        service.FopListDict.Add(key, new() { new() {
            CategoryKey = key,
            IsFamily = true,
            Family = new(){
                CategoryKey = key, Id = famId, Tags = tags.Values.ToList()
            }}
        });
        A.CallTo(() => stat.GetTags()).Returns(tags);
        #endregion

        // Act
        int newPref = 1;
        bool tagAlreadyButtoned = await service.GlobChangePreferenceAsync(tagId, newPref);

        // Assert
        service.FopListDict[key].Should().Contain(fop =>
            !fop.IsFamily && fop.Preference.Tag.Id == tagId && fop.Preference.Preference == newPref);
        tagAlreadyButtoned.Should().BeFalse();
    }
    [Fact]
    public async Task GlobalService_GlobChangePreferenceAsync_UpdatesFOPForButtonedTagWithFamilyReturnsTrue()
    {
        #region Arrange
        string key = "cat";
        Guid tagId = Guid.NewGuid();
        Guid famId = Guid.NewGuid();
        int pref = -1;
        // add a tag that is in a family
        Dictionary<Guid, TagFromDb> tags = new() { { tagId, new() {
            Id = tagId, CategoryKey = key, FamilyId = famId
        } } };
        // add the corresponding family to FopListDict
        service.FopListDict.Add(key, new() { new() {
            CategoryKey = key,
            IsFamily = true,
            Family = new(){
                CategoryKey = key, Id = famId, Tags = tags.Values.ToList()
            }}
        });
        // add a preference for this tag
        service.FopListDict[key].Add(new() {
            CategoryKey = key,
            IsFamily = false,
            Preference = new CatagPreference(){
                Tag=tags[tagId],
                Preference=pref
            }
        });
        A.CallTo(() => stat.GetTags()).Returns(tags);
        #endregion

        // Act
        int newPref = 1;
        bool tagAlreadyButtoned = await service.GlobChangePreferenceAsync(tagId, newPref);

        // Assert
        service.FopListDict[key].Should().Contain(fop =>
            !fop.IsFamily && fop.Preference.Tag.Id == tagId && fop.Preference.Preference == newPref);
        tagAlreadyButtoned.Should().BeTrue();
    }
}
