using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class LoginViewModel : MyBaseViewModel
{
    private readonly IGlobalService globalService;
    private readonly IUsersService usersService;
    private readonly INavigationService navigationService;

    public LoginViewModel(
        IGlobalService globalService,
        IUsersService usersService,
        INavigationService navigationService)
        : base(globalService)
    {
        this.globalService = globalService;
        this.usersService = usersService;
        this.navigationService = navigationService;
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
        UserDto user = null!;

        try
        {
            user = await usersService.GetUserByUsernameAsync(EnteredUsername.Trim());
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("DB Error",
                ex.Message, "Ok");
        }
        
        if (user != null) 
        {
            // str ?? "" //returns "" if str is null and returns str othw.
            if ((EnteredPassword ?? "") == (user.Password ?? ""))
            {
                // Login
                CurrentUser = user;
                await globalService.LoadPreferencesAsync();
                await globalService.LoadIdeaInteractionsAsync();

                navigationService.RedrawNavigationTarget = true;

                //the amount of "/" to prepend depends on the shell's design
                //r.n. I use "///" because the debugger suggested it
                //  3*/ means downward search and full navigation stack replacement
                //That 1*/ does not work seems not to be my bad, but MAUI's
                //according to:
                //  https://github.com/xamarin/Xamarin.Forms/issues/6096
                await navigationService.NavigateToAsync($"///{nameof(FeedPage)}");
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
    public void FailResponse(string text)
    {
        DisplayedLoginResponse = text;
    }
}
