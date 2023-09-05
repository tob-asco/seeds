using Microsoft.Maui.Platform;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Interfaces;
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
        service.FamilyOrPreferences.Should().HaveCount(tags.Count);
        foreach (var tag in tags)
        {
            service.FamilyOrPreferences.Should().Contain(t =>
                t.Preference.Tag.Id == tag.Id);
        }
    }
    [Fact]
    public async Task GlobalService_LoadPreferencesAsync_PopulatesFOPsWithFams()
    {
        // Arrange
        Dictionary<Guid, Family> fams = new()
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
        service.FamilyOrPreferences.Should().HaveCount(fams.Count);
        foreach (var fam in fams)
        {
            service.FamilyOrPreferences.Should().Contain(t =>
                t.Family.Id == fam.Value.Id);
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
        service.GetPreferences().Values.Should().NotContain(up => up.Username == s);
        service.FamilyOrPreferences.Should().NotContain(fop =>
            fop.Preference.Tag.Name == s);
    }
}
