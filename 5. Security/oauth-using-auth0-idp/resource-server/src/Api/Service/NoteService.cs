using Api.Models;
using Bogus;

namespace Api.Service;

public class NoteService : INoteService
{
    private readonly IReadOnlyList<Note> _notes;

    public NoteService()
    {
        _notes = Enumerable.Range(0, 100)
        .Select(_ =>
        {
            return new Faker<Note>()
            .RuleFor(n => n.Id, f => Guid.NewGuid())
            .RuleFor(n => n.Title, f => f.Lorem.Sentence(10))
            .RuleFor(n => n.Content, f => f.Lorem.Sentences(3))
            .Generate();
        })
        .ToList()
        .AsReadOnly();
    }

    public IReadOnlyList<Note> GetNotes()
    {
        return _notes;
    }
}
