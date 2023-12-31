﻿using seeds.Dal.Interfaces;
using seeds1.Factories;
using seeds1.Helpers;
using seeds1.Interfaces;
using seeds1.ViewModel;

namespace seeds1.Tests.ViewModel;

public class PreferencesVmTests
{
    private readonly IStaticService stat;
    private readonly IGlobalService glob;
    private readonly IUserPreferenceService prefService;
    private readonly PreferencesViewModel vm;

    public PreferencesVmTests()
    {
        stat = A.Fake<IStaticService>();
        glob = A.Fake<IGlobalService>();
        prefService = A.Fake<IUserPreferenceService>();
        vm = new(stat, glob, A.Fake<IGenericFactory<FamilyPopupViewModel>>(),
            new PopupSizeConstants(A.Fake<IDeviceDisplay>()), prefService);
    }

    [Fact]
    public async Task PreferencesVm_ChangeTopicPreference_ChangesOnlyKeyedPreference()
    {
        //#region Arrange
        //int pref1 = 0, pref0 = -1;
        //vm.FopGroups = new() { new() // both in first group
        //{
        //    new() { Preference = new() { Topic = new(){CategoryKey = "Cat0" }, Preference = pref0 } },
        //    new() { Preference = new() { Topic = new(){CategoryKey = "Cat1" }, Preference = pref1 } },
        //}};
        //A.CallTo(() => prefService.StepPreference(A<int>.Ignored))
        //    .Returns(14);
        //#endregion

        //// Act
        //await vm.ChangeTopicPreference(vm.FopGroups[0][1].Preference);

        //// Assert
        //vm.FopGroups[0][0].Preference.Preference.Should().Be(pref0);
        //vm.FopGroups[0][1].Preference.Preference.Should().NotBe(pref1);
    }

}

