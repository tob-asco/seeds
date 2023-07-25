using seeds.Dal.Dto.ToAndFromDb;

namespace seeds1.Interfaces;

public interface IGlobalService
{
    public UserDto CurrentUser { get; set; }
}
