using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudyMate.DTOs;
using StudyMate.DTOs.Note;
using StudyMate.Services.Interfaces;

namespace StudyMate.Controllers;
[Authorize(Roles = "User")]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateNote([FromBody] CreateNote createNote)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ServiceResponse(Message: "Invalid model state", IsSuccess: false));
        var result = await _noteService.CreateNoteAsync(createNote);
        if (result is null)
            return BadRequest(new ServiceResponse(Message: "Failed to create note", IsSuccess: false));
        return CreatedAtAction(nameof(GetNoteById), new { noteId = result.Id }, result);
      
    }
    [HttpGet("{noteId}")]
    public async Task<IActionResult> GetNoteById(string noteId)
    {
        var result = await _noteService.GetNoteByIdAsync(noteId);
        if (result is null)
            return NotFound(new ServiceResponse(Message: "Note not found", IsSuccess: false));
        return Ok(result);
    }
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetNotesByUserId(string userId)
    {
        var result = await _noteService.GetNotesByUserIdAsync(userId);
        if (result is null)
            return NotFound(new ServiceResponse(Message: "No notes found for this user", IsSuccess: false));
        return Ok(result);
    }
    [HttpPut]
    public async Task<IActionResult> UpdateNote([FromBody] UpdateNote updateNote)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ServiceResponse(Message: "Invalid model state", IsSuccess: false));
        var result = await _noteService.UpdateNoteAsync(updateNote);
        if (!result)
            return NotFound(new ServiceResponse(Message: "Failed to update note", IsSuccess: false));
        return NoContent();
    }
    [HttpDelete("{noteId}")]
    public async Task<IActionResult> DeleteNote(string noteId)
    {
        var result = await _noteService.DeleteNoteAsync(noteId);
        if (!result)
            return NotFound(new ServiceResponse(Message: "Failed to delete note", IsSuccess: false));
        return NoContent();
    }
   
}
 