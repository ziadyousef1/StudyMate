using AutoMapper;
using StudyMate.DTOs.Note;
using StudyMate.Models;

namespace StudyMate.Mappers;

public class NoteMappingConfig : Profile
{
    public NoteMappingConfig()
    {
        CreateMap<CreateNote, Note>();
        CreateMap<Note, GetNote>();
        CreateMap<UpdateNote, Note>();
    }
}