namespace StudyMate.Repositories.Interfaces;

public interface INoteRepository
{
    Task<Note> CreateAsync(Note note);
    Task<bool> UpdateAsync(Note note);
    Task<bool> DeleteAsync(string noteId);
    Task<Note> GetByIdAsync(string noteId);
    Task<IEnumerable<Note>> GetByUserIdAsync(string userId);
}