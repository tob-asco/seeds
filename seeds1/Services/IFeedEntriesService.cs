using seeds1.MauiModels;

namespace seeds1.Services;

public interface IFeedEntriesService
{
    public Task<List<FeedEntry>> GetFeedEntriesPaginated(int page, int maxPageSize);
}
