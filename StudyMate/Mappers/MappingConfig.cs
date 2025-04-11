using AutoMapper;
using StudyMate.DTOs.Authentication;
using StudyMate.DTOs.Note;
using StudyMate.DTOs.Profile;
using StudyMate.Models;

namespace StudyMate.Mappers;

public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<RegisterUser, AppUser>();
        CreateMap<AppUser, RegisterUser>();
        CreateMap<LoginUser, AppUser>();
        CreateMap<AppUser, LoginUser>();
        CreateMap<AppUser, GetUser>();
        CreateMap<UpdateUser, AppUser>();
        CreateMap<AppUser, UpdateUser>();
        CreateMap<CreateUser, AppUser>();
        CreateMap<CreateNote, Note>();
        CreateMap<Note, GetNote>();
        CreateMap<UpdateNote, Note>();

        


    }
}