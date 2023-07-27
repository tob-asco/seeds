using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToDb;

namespace seeds.Dal.Interfaces;

public interface IIdeasService
{
    /* If base returns null, throws.
     */
    public Task<IdeaFromDb> GetIdeaAsync(int id);
    /* Returns list of length >= 0 or throws.
     */
    public Task<List<IdeaFromDb>> GetIdeasPaginatedAsync(
        int pageIndex, int pageSize = 5,
        string orderByColumn = nameof(IdeaFromDb.CreationTime),
        bool isDescending = true);
    /* Success or throw.
     */
    public Task<IdeaFromDb> PostIdeaAsync(IdeaToDb idea);

}
