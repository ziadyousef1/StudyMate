using AutoMapper;
using StudyMate.DTOs.Authentication;
using StudyMate.Models;

namespace StudyMate.Mappers;

public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<CreateUser, AppUser>();
        CreateMap<AppUser, CreateUser>();
        CreateMap<LoginUser, AppUser>();
        CreateMap<AppUser, LoginUser>();
    }
}