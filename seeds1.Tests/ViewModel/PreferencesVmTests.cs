using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.ViewModel;

namespace seeds1.Tests.ViewModel;

public class PreferencesVmTests
{
    private readonly ICatagPreferencesService catPrefService;
    private readonly ICatagUserPreferenceService cupService;
    private readonly PreferencesViewModel vm;

    public PreferencesVmTests()
    {
        catPrefService = A.Fake<ICatagPreferencesService>();
        cupService = A.Fake<ICatagUserPreferenceService>();
        vm = new(A.Fake<IGlobalService>(), catPrefService, cupService);
    }

    [Fact]
    public async Task PreferencesVm_ChangeCategoryPreference_ChangesOnlyKeyedPreference()
    {
        #region Arrange
        int pref1 = 0, pref0 = -1;
        vm.CatPrefs = new()
        {
            new() { CategoryKey = "Cat0", Preference = pref0 },
            new() { CategoryKey = "Cat1", Preference = pref1 },
        };
        A.CallTo(() => catPrefService.StepPreference(A<int>.Ignored))
            .Returns(14);
        A.CallTo(() => cupService.PutCatagUserPreferenceAsync(
            A<string>.Ignored, A<string>.Ignored, A<int>.Ignored, A<string?>.Ignored))
            .Returns(true);
        #endregion

        // Act
        await vm.ChangeCategoryPreference(vm.CatPrefs[1].CategoryKey);

        // Assert
        vm.CatPrefs[0].Preference.Should().Be(pref0);
        vm.CatPrefs[1].Preference.Should().NotBe(pref1);
    }

}

