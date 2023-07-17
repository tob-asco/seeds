using FakeItEasy;
using seeds.Dal.Interfaces;
using seeds.Dal.Model;
using seeds1.Services;
using seeds1.Tests.Services;
using seeds1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds1.Tests.ViewModel;

public class LoginViewModelTests
{
    private readonly IUsersService _userService;

    public LoginViewModelTests()
    {
        _userService = A.Fake<IUsersService>();
    }

    #region Positive Tests
    [Fact]
    public async Task LoginViewModel_LoginCommand_NavigatesToFeed()
    {
        #region Arrange
        MockNavigationService navigationService = new();
        LoginViewModel vm = new(_userService, navigationService);
        var enteredUsername = "testuser";
        var enteredPassword = "password";
        var user = new User
        {
            Username = "testuser",
            Password = "password"
        };

        A.CallTo(() => _userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navigationService.NavigationCalled.Should().BeTrue();
    }

    [Fact]
    public async Task LoginViewModel_LoginCommand_NavigatesToFeedEmptyPw()
    {
        #region Arrange
        MockNavigationService navigationService = new();
        LoginViewModel vm = new(_userService, navigationService);
        var enteredUsername = "testuser";
        var enteredPassword = "";
        var user = new User
        {
            Username = "testuser",
            Password = null
        };

        A.CallTo(() => _userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navigationService.NavigationCalled.Should().BeTrue();
    }

    [Fact]
    public async Task LoginViewModel_LoginCommand_NavigatesToFeedNullPw()
    {
        #region Arrange
        MockNavigationService navigationService = new();
        LoginViewModel vm = new(_userService, navigationService);
        var enteredUsername = "testuser";
        var user = new User
        {
            Username = "testuser",
            Password = ""
        };

        A.CallTo(() => _userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        //vm.EnteredPassword = null;
        await vm.Login();

        // Assert
        navigationService.NavigationCalled.Should().BeTrue();
    }

    #endregion

    #region Negative Tests
    [Fact]
    public async Task LoginViewModel_LoginCommand_IfInvalidCredentialsNoNavigationButDisplayMessage()
    {
        #region Arrange
        MockNavigationService navigationService = new();
        LoginViewModel vm = new(_userService, navigationService);
        vm.DisplayedLoginResponse = "";
        var enteredUsername = "testuser";
        var enteredPassword = "password";
        var user = new User
        {
            Username = "testuser",
            Password = "PASSWORD"
        };

        A.CallTo(() => _userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navigationService.NavigationCalled.Should().BeFalse();
        vm.DisplayedLoginResponse.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task LoginViewModel_LoginCommand_IfNoUsernameNoNavigationButDisplayMessage()
    {
        #region Arrange
        MockNavigationService navigationService = new();
        LoginViewModel vm = new(_userService, navigationService);
        vm.DisplayedLoginResponse = "";
        var enteredUsername = "";
        var enteredPassword = "password";
        var user = new User
        {
            Username = "",
            Password = "password"
        };

        A.CallTo(() => _userService.GetUserByUsernameAsync(A<string>.Ignored))
            .Returns(user);
        #endregion

        // Act
        vm.EnteredUsername = enteredUsername;
        vm.EnteredPassword = enteredPassword;
        await vm.Login();

        // Assert
        navigationService.NavigationCalled.Should().BeFalse();
        vm.DisplayedLoginResponse.Should().NotBeNullOrEmpty();
    }

    #endregion
}
