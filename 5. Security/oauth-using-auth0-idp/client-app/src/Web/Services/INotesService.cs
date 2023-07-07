using Web.Models;

namespace Web.Services;

public interface INotesService
{
    Task<IReadOnlyList<Note>> GetNotesAsync();
}
