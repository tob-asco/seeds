using seeds.Dal.Dto.FromDb;
using seeds1.MauiModels;

namespace seeds1.Interfaces;

public interface IFeedEntriesService
{
    public Task<List<UserFeedentry>> GetFeedEntriesPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime), bool isDescending = true);
}
