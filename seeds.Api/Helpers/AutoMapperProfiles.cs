using AutoMapper;
using seeds.Dal.Dto.ToApi;
using seeds.Dal.Model;

namespace seeds.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<IdeaDtoApi, Idea>();
        CreateMap<Idea, IdeaDtoApi>();
        CreateMap<User, UserDtoApi>();
        CreateMap<UserDtoApi, User>();
    }
}
