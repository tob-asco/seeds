using seeds1.Interfaces;

namespace seeds1.Factories;

public class FamilyPopupViewModelFactory : IGenericFactory<FamilyPopupViewModel>
{
    private readonly IServiceProvider serviceProvider;

    public FamilyPopupViewModelFactory(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public FamilyPopupViewModel Create()
    {
        return new FamilyPopupViewModel(
            serviceProvider.GetRequiredService<IStaticService>());
    }
}
