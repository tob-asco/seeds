using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.Factories;

public class LoginViewModelFactory : IGenericFactory<LoginViewModel>
{
    private readonly IServiceProvider serviceProvider;

    public LoginViewModelFactory(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public LoginViewModel Create()
    {
        return new LoginViewModel(
            serviceProvider.GetRequiredService<IGlobalService>(),
            serviceProvider.GetRequiredService<IUsersService>(),
            serviceProvider.GetRequiredService<INavigationService>());
    }
}
