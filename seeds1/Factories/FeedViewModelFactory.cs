using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.Factories;

public class FeedViewModelFactory : IGenericFactory<FeedViewModel>
{
    private readonly IServiceProvider serviceProvider;

    public FeedViewModelFactory(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public FeedViewModel Create()
    {
        return new FeedViewModel(
            serviceProvider.GetRequiredService<IGlobalService>(),
            serviceProvider.GetRequiredService<IGenericFactory<FeedEntryViewModel>>(),
            serviceProvider.GetRequiredService<IFeedEntriesService>(),
            serviceProvider.GetRequiredService<ICatagUserPreferenceService>(),
            serviceProvider.GetRequiredService<ICatagPreferencesService>()
            );
    }
}
