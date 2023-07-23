using seeds.Dal.Dto.ToApi;

namespace seeds.Dal.Interfaces;

public interface IIdeasService
{
    /* If base returns null, throws.
     */
    public Task<IdeaDtoApi> GetIdeaAsync(int id);
    /* Can return null as there might be no more ideas
     */
    public Task<List<IdeaDtoApi>?> GetIdeasPaginatedAsync(int page, int maxPageSize);

}
