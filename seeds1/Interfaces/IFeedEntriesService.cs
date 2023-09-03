using seeds.Dal.Dto.FromDb;
using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface IFeedEntriesService
{
    public Task<List<UserFeedentry>> GetFeedEntriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime),
        bool isDescending = true);

    /* This method uses the feedentryPage endpoint of IdeasController.
     * So it accesses far less endpoints than the above method.
     */
    public Task<List<UserFeedentry>> GetUserFeedentriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime),
        bool isDescending = true);
}
