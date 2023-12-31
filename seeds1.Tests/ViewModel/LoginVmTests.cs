﻿using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;
using seeds1.Tests.Services;
using seeds1.ViewModel;

namespace seeds1.Tests.ViewModel;

public class LoginVmTests
{
    private readonly IStaticService staticService;
    private readonly IGlobalService globalService;
    private readonly IUsersService userService;
    private readonly INavigationService navService;
    private readonly LoginViewModel vm;

    public LoginVmTests()
    {
        staticService = A.Fake<IStaticService>();
        globalService = A.Fake<IGlobalService>();
        userService = A.Fake<IUsersService>();
        navService = (INavigationService)new MockNavigationService();
        vm = new(staticService, globalService, userService, navService);
    }

    #region Positive Tests
    [Fact]
    public async Task LoginViewModel_LoginCommand_NavigatesToFeed()
    {
        #region Arrange
        var enteredUsername = "testuser";
        var enteredPassword = "password";
        var user = new UserDto
        {
            Username = "testuser",
            Password = "password"
        };

        A.CallTo(() => userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navService.NavigationCalled.Should().BeTrue();
    }

    [Fact]
    public async Task LoginViewModel_LoginCommand_NavigatesToFeedEmptyPw()
    {
        #region Arrange
        var enteredUsername = "testuser";
        var enteredPassword = "";
        var user = new UserDto
        {
            Username = "testuser",
            Password = null
        };

        A.CallTo(() => userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navService.NavigationCalled.Should().BeTrue();
    }

    [Fact]
    public async Task LoginViewModel_LoginCommand_NavigatesToFeedNullPw()
    {
        #region Arrange
        var enteredUsername = "testuser";
        var user = new UserDto
        {
            Username = "testuser",
            Password = ""
        };

        A.CallTo(() => userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        //vm.EnteredPassword = null;
        await vm.Login();

        // Assert
        navService.NavigationCalled.Should().BeTrue();
    }

    #endregion

    #region Negative Tests
    [Fact]
    public async Task LoginViewModel_LoginCommand_IfInvalidCredentialsNoNavigationButDisplayMessage()
    {
        #region Arrange
        vm.DisplayedLoginResponse = "";
        var enteredUsername = "testuser";
        var enteredPassword = "password";
        var user = new UserDto
        {
            Username = "testuser",
            Password = "PASSWORD"
        };

        A.CallTo(() => userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navService.NavigationCalled.Should().BeFalse();
        vm.DisplayedLoginResponse.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginViewModel_LoginCommand_IfNoUsernameNoNavigationButDisplayMessage()
    {
        #region Arrange
        vm.DisplayedLoginResponse = "";
        var enteredUsername = "";
        var enteredPassword = "password";
        var user = new UserDto
        {
            Username = "",
            Password = "password"
        };

        A.CallTo(() => userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navService.NavigationCalled.Should().BeFalse();
        vm.DisplayedLoginResponse.Should().NotBeNullOrEmpty();
    }

    #endregion
}
