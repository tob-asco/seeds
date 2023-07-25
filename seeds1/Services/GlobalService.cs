using seeds.Dal.Dto.ToAndFromDb;
using seeds1.Interfaces;

namespace seeds1.Services;

public class GlobalService : IGlobalService
{
    public UserDto CurrentUser { get; set; }
}
