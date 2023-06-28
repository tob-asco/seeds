using seeds.Dal.Model;
using seeds.Dal.Services;

namespace seeds1.ViewModel;

public partial class LoginViewModel : BasisViewModel
{
    private readonly IUsersService _usersService;

    public LoginViewModel(IUsersService usersService)
    {
        _usersService = usersService;
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
        if (EnteredUsername == null)
        {
            FailResponse("We need a username here.");
            return;
        }
        FailResponse("Checking..."); //shouldnt be a "fail" response..
        //look up DB for existence of entered data:
        User user = await _usersService.GetUserByUsername(EnteredUsername.Trim());
        
        if (user != null) 
        {
            // str ?? "" //returns "" if str is null and returns str otw.
            if ((EnteredPassword ?? "") == (user.Password ?? ""))
            {
                // Login
                
                // pass only a unique identifier (security, scalability)
                var navigationParameters = new Dictionary<string, object>
                {
                    { nameof(User.Username), user.Username }
                };

                //the amount of "/" to prepend depends on the shell's design
                //r.n. I use "///" because the debugger suggested it
                //  3*/ means downward search and full navigation stack replacement
                //That 1*/ does not work seems not to be my bad, but MAUI's
                //according to:
                //  https://github.com/xamarin/Xamarin.Forms/issues/6096
                await Shell.Current.GoToAsync("///FeedPage", true, navigationParameters);

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
