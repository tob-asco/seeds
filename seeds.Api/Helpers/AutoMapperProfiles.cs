﻿using AutoMapper;
using seeds.Dal.Dto.FromDb;
using seeds.Dal.Model;

namespace seeds.Api.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<IdeaFromDb, Idea>();
        CreateMap<Idea, IdeaFromDb>();
        CreateMap<User, UserFromDb>();
        CreateMap<UserFromDb, User>();
        CreateMap<Category, CategoryFromDb>();
        CreateMap<CategoryFromDb, Category>();
    }
}
