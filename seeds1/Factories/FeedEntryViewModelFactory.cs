using seeds.Dal.Interfaces;
using seeds1.Interfaces;

namespace seeds1.Factories;

public class FeedEntryViewModelFactory : IFeedEntryViewModelFactory
{
    private readonly IServiceProvider serviceProvider;

    public FeedEntryViewModelFactory(
        IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }
    public FeedEntryViewModel CreateFeedEntryViewModel()
    {
        return new FeedEntryViewModel(
            serviceProvider.GetRequiredService<IGlobalService>(),
            serviceProvider.GetRequiredService<IUserIdeaInteractionService>()
            );
    }
}
