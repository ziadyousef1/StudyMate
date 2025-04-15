using AutoMapper;
using StudyMate.DTOs.Profile;
using StudyMate.Models;

namespace StudyMate.Mappers;

public class UserProfileMappingConfig : Profile
{
    public UserProfileMappingConfig()
    {
        CreateMap<AppUser, GetUser>();
        CreateMap<UpdateUser, AppUser>();
        CreateMap<AppUser, UpdateUser>();
        CreateMap<CreateUser, AppUser>();
    }
}