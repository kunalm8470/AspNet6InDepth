using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Models;
using Web.Services;

namespace Web.Controllers;

public class NotesController : Controller
{
    private readonly INotesService _notesService;

    public NotesController(INotesService notesService)
    {
        _notesService = notesService;
    }

    [Authorize]
    public async Task<IActionResult> Index()
    {
        IReadOnlyList<Note> notes = await _notesService.GetNotesAsync();

        return View(notes);
    }
}
