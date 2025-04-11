using Microsoft.AspNetCore.Http.HttpResults;
using StudyMate.DTOs.Note;

namespace StudyMate.Services.Interfaces;

public interface INoteService
{
    Task<GetNote> CreateNoteAsync(CreateNote note);
    Task<bool> UpdateNoteAsync(UpdateNote note);
    Task<bool> DeleteNoteAsync(string noteId);
    Task<GetNote> GetNoteByIdAsync(string noteId);
    Task<IEnumerable<GetNote>> GetNotesByUserIdAsync(string userId);
    
}
