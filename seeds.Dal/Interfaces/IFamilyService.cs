using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IFamilyService
{
    public Task<List<FamilyFromDb>> GetFamiliesAsync();
}
