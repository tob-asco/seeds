using seeds.Dal.Dto.ToApi;
using seeds1.Interfaces;

namespace seeds1.Services;

public class GlobalVmService : IGlobalVmService
{
    public UserDtoApi CurrentUser { get; set; }
}
