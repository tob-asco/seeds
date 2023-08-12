using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.Factories;

public class PreferencesViewModelFactory : IGenericFactory<PreferencesViewModel>
{
    private readonly IServiceProvider serviceProvider;

    public PreferencesViewModelFactory(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public PreferencesViewModel Create()
    {
        return new PreferencesViewModel(
            serviceProvider.GetRequiredService<IGlobalService>(),
            serviceProvider.GetRequiredService<ICatagPreferencesService>(),
            serviceProvider.GetRequiredService<ICatagUserPreferenceService>()
            );
    }
}
