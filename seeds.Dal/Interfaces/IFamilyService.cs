using seeds.Dal.Model;

namespace seeds.Dal.Interfaces;

public interface IFamilyService
{
    public Task<List<Family>> GetFamiliesAsync();
}
