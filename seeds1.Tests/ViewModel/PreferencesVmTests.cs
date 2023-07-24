using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.ViewModel;

namespace seeds1.Tests.ViewModel;

public class PreferencesVmTests
{
    private readonly ICategoryPreferencesService catPrefService;
    private readonly ICategoryUserPreferenceService cupService;
    private readonly PreferencesViewModel vm;

    public PreferencesVmTests()
    {
        catPrefService = A.Fake<ICategoryPreferencesService>();
        cupService = A.Fake<ICategoryUserPreferenceService>();
        vm = new(A.Fake<IGlobalService>(), catPrefService, cupService);
    }

    [Fact]
    public async Task PreferencesVm_ChangeCategoryPreference_ChangesOnlyKeyedPreference()
    {
        #region Arrange
        int pref1 = 0, pref0 = -1;
        vm.CatPrefs = new()
        {
            new() { Key = "Cat0", Value = pref0 },
            new() { Key = "Cat1", Value = pref1 },
        };
        A.CallTo(() => catPrefService.StepCatPreference(A<int>.Ignored))
            .Returns(14);
        A.CallTo(() => cupService.PutCategoryUserPreferenceAsync(
            A<string>.Ignored, A<string>.Ignored, A<int>.Ignored))
            .Returns(true);
        #endregion

        // Act
        await vm.ChangeCategoryPreference(vm.CatPrefs[1].Key);

        // Assert
        vm.CatPrefs[0].Value.Should().Be(pref0);
        vm.CatPrefs[1].Value.Should().NotBe(pref1);
    }

}

