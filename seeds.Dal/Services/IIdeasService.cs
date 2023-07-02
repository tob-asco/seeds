using seeds.Dal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace seeds.Dal.Services;

public interface IIdeasService
{
    public Task<List<Idea>> GetIdeas();
    public Task<List<Idea>> GetIdeas(int page, int pageSize);
}
