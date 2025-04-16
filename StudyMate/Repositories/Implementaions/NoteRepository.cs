namespace StudyMate.Repositories.Implementaions;

public class NoteRepository(ApplicationDbContext context) : INoteRepository
{
    public async Task<Note> CreateAsync(Note note)
    {
        var result = await context.Notes.AddAsync(note);
        if (result.State == EntityState.Added)
        {
            await context.SaveChangesAsync();
            return note;
        }
        return null;
        
    }

    public async Task<bool> UpdateAsync(Note note)
    {
        var existingNote =await context.Notes.FindAsync(note.Id);
        if (existingNote is null)
            return false; 
        if (!string.IsNullOrEmpty(note.Title))
            existingNote.Title = note.Title;
        
        if (!string.IsNullOrEmpty(note.Content))
            existingNote.Content = note.Content;
        await context.SaveChangesAsync();
        return true;
        
    }
    public async Task<bool> DeleteAsync(string noteId)
    {
        var note = context.Notes.Find(noteId);
        if (note is not null)
        {
            context.Notes.Remove(note); 
            await context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<Note> GetByIdAsync(string noteId)
    {
        var note = await context.Notes.FindAsync(noteId);
        if (note is null)
            return null;
        return note;
    }
    

    public async Task<IEnumerable<Note>> GetByUserIdAsync(string userId)
    {
        return await context.Notes
            .Where(n => n.UserId == userId)
            .ToListAsync();
        
    }
}