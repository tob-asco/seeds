using seeds.Dal.Dto.ToApi;
using seeds1.Interfaces;

namespace seeds1.Services;

public class GlobalService : IGlobalService
{
    public UserDtoApi CurrentUser { get; set; }
}
