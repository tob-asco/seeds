using seeds.Dal.Dto.FromDb;

namespace seeds1.Interfaces;

public interface IGlobalService
{
    public UserFromDb CurrentUser { get; set; }
}
