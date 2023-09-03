using seeds.Dal.Dto.FromDb;
using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface IFeedEntriesService
{
    /* This method uses the feedentryPage endpoint of IdeasController.
     * So it accesses endpoints rarely.
     */
    public Task<List<UserFeedentry>> GetUserFeedentriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime),
        bool isDescending = true);
}
