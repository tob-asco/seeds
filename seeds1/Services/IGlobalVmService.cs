using seeds.Dal.Dto.ToApi;

namespace seeds1.Services;

public interface IGlobalVmService
{
    public UserDtoApi CurrentUser { get; set; }
}
