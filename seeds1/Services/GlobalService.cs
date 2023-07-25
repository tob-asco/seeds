using seeds.Dal.Dto.FromDb;
using seeds1.Interfaces;

namespace seeds1.Services;

public class GlobalService : IGlobalService
{
    public UserFromDb CurrentUser { get; set; }
}
