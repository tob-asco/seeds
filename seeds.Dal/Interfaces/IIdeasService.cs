using seeds.Dal.Dto.FromDb;

namespace seeds.Dal.Interfaces;

public interface IIdeasService
{
    /* If base returns null, throws.
     */
    public Task<IdeaFromDb> GetIdeaAsync(int id);
    /* Can return null as there might be no more ideas
     */
    public Task<List<IdeaFromDb>?> GetIdeasPaginatedAsync(int page, int maxPageSize);

}
