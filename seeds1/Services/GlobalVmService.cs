using seeds.Dal.Dto.ToApi;

namespace seeds1.Services;

public class GlobalVmService : IGlobalVmService
{
    public UserDtoApi CurrentUser { get; set; }
}
