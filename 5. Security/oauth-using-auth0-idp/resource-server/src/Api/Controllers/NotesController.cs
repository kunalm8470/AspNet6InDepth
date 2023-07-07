using Api.Models;
using Api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
	{
        _noteService = noteService;
    }

    [HttpGet]
    [Authorize(Policy = "read:notes")]
    public ActionResult<IReadOnlyList<Note>> GetNotes()
    {
        IReadOnlyList<Note> notes = _noteService.GetNotes();

        return Ok(notes);
    }
}
