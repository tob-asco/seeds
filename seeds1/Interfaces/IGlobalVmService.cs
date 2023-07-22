using seeds.Dal.Dto.ToApi;

namespace seeds1.Interfaces;

public interface IGlobalVmService
{
    public UserDtoApi CurrentUser { get; set; }
}
