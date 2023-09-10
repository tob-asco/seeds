using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.ViewModel;

public partial class LoginViewModel : MyBaseViewModel
{
    private readonly IStaticService stat;
    private readonly IGlobalService glob;
    private readonly IUsersService usersService;
    private readonly INavigationService navigationService;

    public LoginViewModel(
        IStaticService stat,
        IGlobalService glob,
        IUsersService usersService,
        INavigationService navigationService)
        : base(stat, glob)
    {
        this.stat = stat;
        this.glob = glob;
        this.usersService = usersService;
        this.navigationService = navigationService;
    }

    [ObservableProperty]
    string enteredUsername;
    [ObservableProperty]
    string enteredPassword;
    [ObservableProperty]
    string displayedLoginResponse;
    [ObservableProperty]
    Color colorLoginResponse = Color.Parse("red");

    [RelayCommand]
    public async Task Login()
    {
        if ((EnteredUsername ?? "") == "")
        {
            LoginResponse("We need a username here.");
            return;
        }
        LoginResponse("Checking...", isFail: false, isSuccess: false);
        UserDto user = null!;

        try
        {
            user = await usersService.GetUserByUsernameAsync(EnteredUsername.Trim());
        }
        catch
        {
            LoginResponse("Server error. Please try again.");
            return;
        }
        
        if (user != null) 
        {
            // str ?? "" //returns "" if str is null and returns str othw.
            if ((EnteredPassword ?? "") == (user.Password ?? ""))
            {
                LoginResponse("Logging in...", isFail: false, isSuccess: true);

                // load statics (needed if OnStart's loading failed)
                await stat.LoadStaticsAsync();
                
                // set and load globals
                glob.CurrentUser = user;
                await glob.LoadPreferencesAsync();
                await glob.LoadIdeaInteractionsAsync();
                await glob.MoreFeedentriesAsync();

                // global switch to inform NavigatedTo page
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
                LoginResponse("Invalid Credentials.");
            }
        }
        else
        {
            // username not existing
            LoginResponse("Invalid Credentials.");
        }
    }
    public void LoginResponse(string text, bool isFail = true, bool isSuccess = false)
    {
        DisplayedLoginResponse = text;
        if (isFail && !isSuccess) { ColorLoginResponse = Color.Parse("red"); }
        else if(!isFail && !isSuccess) { ColorLoginResponse = Color.Parse("lightgray"); }
        else { ColorLoginResponse = Color.Parse("green"); }
    }
}
