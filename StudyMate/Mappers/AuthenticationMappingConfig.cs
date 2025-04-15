using AutoMapper;
using StudyMate.DTOs.Authentication;
using StudyMate.Models;

namespace StudyMate.Mappers;

public class AuthenticationMappingConfig : Profile
{
    public AuthenticationMappingConfig()
    {
        CreateMap<RegisterUser, AppUser>();
        CreateMap<AppUser, RegisterUser>();
        CreateMap<LoginUser, AppUser>();
        CreateMap<AppUser, LoginUser>();
    }
}
