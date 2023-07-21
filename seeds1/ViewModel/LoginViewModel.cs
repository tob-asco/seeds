using seeds.Dal.Dto.ToApi;
using seeds.Dal.Interfaces;
using seeds1.Services;

namespace seeds1.ViewModel;

public partial class LoginViewModel : BasisViewModel
{
    private readonly IGlobalVmService globalService;
    private readonly IUsersService _usersService;
    private readonly INavigationService _navigationService;

    public LoginViewModel(
        IGlobalVmService globalService,
        IUsersService usersService,
        INavigationService navigationService)
        : base(globalService)
    {
        this.globalService = globalService;
        _usersService = usersService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    string enteredUsername;
    [ObservableProperty]
    string enteredPassword;
    [ObservableProperty]
    string displayedLoginResponse;

    [RelayCommand]
    public async Task Login()
    {
        if ((EnteredUsername ?? "") == "")
        {
            FailResponse("We need a username here.");
            return;
        }
        FailResponse("Checking..."); //shouldnt be a "fail" response..
        //look up DB for existence of entered data:
        UserDtoApi user = await _usersService.GetUserByUsernameAsync(EnteredUsername.Trim());
        
        if (user != null) 
        {
            // str ?? "" //returns "" if str is null and returns str othw.
            if ((EnteredPassword ?? "") == (user.Password ?? ""))
            {
                // Login
                CurrentUser = user;
                // pass full user object, not just username
                var navParameters = new Dictionary<string, object>
                {
                    //{ nameof(CurrentUser), user },
                    { nameof(RedrawPage), true }
                };

                //the amount of "/" to prepend depends on the shell's design
                //r.n. I use "///" because the debugger suggested it
                //  3*/ means downward search and full navigation stack replacement
                //That 1*/ does not work seems not to be my bad, but MAUI's
                //according to:
                //  https://github.com/xamarin/Xamarin.Forms/issues/6096
                await _navigationService.NavigateToAsync(
                    $"///{nameof(FeedPage)}", navParameters);

                Cleanup();
            }
            else
            {
                // wrong password
                FailResponse("Invalid Credentials.");
            }
        }
        else
        {
            // username not existing
            FailResponse("Invalid Credentials.");
        }
    }

    private void Cleanup()
    {
        EnteredUsername = "";
        EnteredPassword = "";
        DisplayedLoginResponse = "";
    }

    public void FailResponse(string text)
    {
        DisplayedLoginResponse = text;
    }
}
