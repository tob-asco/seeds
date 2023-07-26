using AutoMapper;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Dto.ToAndFromDb;
using seeds.Dal.Dto.ToDb;
using seeds.Dal.Model;

namespace seeds.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<IdeaToDb, Idea>(); // for POST
        CreateMap<IdeaFromDb, Idea>(); // for PUT
        CreateMap<Idea, IdeaFromDb>(); // for GET
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
    }
}
