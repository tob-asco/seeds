using seeds.Dal.Dto.ToApi;

namespace seeds1.Interfaces;

public interface IGlobalService
{
    public UserDtoApi CurrentUser { get; set; }
}
