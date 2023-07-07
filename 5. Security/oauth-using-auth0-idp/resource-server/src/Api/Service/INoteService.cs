using Api.Models;

namespace Api.Service;

public interface INoteService
{
    IReadOnlyList<Note> GetNotes();
}
