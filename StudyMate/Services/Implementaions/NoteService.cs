using AutoMapper;
using StudyMate.DTOs.Note;
using StudyMate.Models;
using StudyMate.Repositories.Interfaces;
using StudyMate.Services.Interfaces;

namespace StudyMate.Services.Implementaions;

public class NoteService:INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public NoteService(INoteRepository noteRepository,IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }


    public async Task<GetNote> CreateNoteAsync(CreateNote note)
    {
        var mappedNote = _mapper.Map<Note>(note);
        mappedNote.CreatedAt = DateTime.UtcNow;
        mappedNote.UpdatedAt = DateTime.UtcNow;
        mappedNote.Id = Guid.NewGuid().ToString();
        var createdNote = await _noteRepository.CreateAsync(mappedNote);
        if (createdNote is null)
            return null;
        var mappedCreatedNote = _mapper.Map<GetNote>(createdNote);
        return mappedCreatedNote;
        
    }

    public async Task<bool> UpdateNoteAsync(UpdateNote note)
    {
        var mappedNote = _mapper.Map<Note>(note);
        mappedNote.UpdatedAt = DateTime.UtcNow;
        return await _noteRepository.UpdateAsync(mappedNote);
    }

    public async Task<bool> DeleteNoteAsync(string noteId)
    {
        var note = await _noteRepository.GetByIdAsync(noteId);
        if (note is null)
            return false;
        return await _noteRepository.DeleteAsync(noteId);
       
    }

    public async Task<GetNote> GetNoteByIdAsync(string noteId)
    {
        var note = await _noteRepository.GetByIdAsync(noteId);
        if (note is null)
            return null;
        var mappedNote = _mapper.Map<GetNote>(note);
        return mappedNote;
    }

    public async Task<IEnumerable<GetNote>> GetNotesByUserIdAsync(string userId)
    {
        var notes =await _noteRepository.GetByUserIdAsync(userId);
        if (notes is null)
            return null;
        var mappedNotes = _mapper.Map<IEnumerable<GetNote>>(notes);
        return mappedNotes;
    }
}