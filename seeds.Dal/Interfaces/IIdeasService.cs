using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Interfaces;

public interface IIdeasService
{
    //public Task<List<Idea>> GetIdeas();
    public Task<Idea?> GetIdeaAsync(int id);
    public Task<List<Idea>?> GetIdeasPaginatedAsync(int page, int maxPageSize);
    public Task<bool> VoteIdeaAsync(int id, int updown);

}
