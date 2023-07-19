using seeds.Dal.Dto.ToApi;

namespace seeds.Dal.Interfaces;

public interface IIdeasService
{
    //public Task<List<Idea>> GetIdeas();
    public Task<IdeaDtoApi?> GetIdeaAsync(int id);
    public Task<List<IdeaDtoApi>?> GetIdeasPaginatedAsync(int page, int maxPageSize);

}
