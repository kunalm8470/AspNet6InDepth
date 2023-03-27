using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Collections.Concurrent;

namespace Infrastructure.Data.Repositories;

public class InMemoryPersonRepository : IInMemoryPersonRepository
{
    private readonly ConcurrentDictionary<Guid, Person> _persons;

    public InMemoryPersonRepository()
    {
        _persons = new ConcurrentDictionary<Guid, Person>();
    }

    public Task AddPersonAsync(Person person)
    {
        if (_persons.ContainsKey(person.Id))
        {
            throw new PersonAlreadyExistsException($"Person already exists with id {person.Id}");
        }

        _persons[person.Id] = person;

        return Task.CompletedTask;
    }

    public Task DeletePersonAsync(Guid id)
    {
        _persons.TryRemove(id, out _);

        return Task.CompletedTask;
    }

    public Task<IEnumerable<Person>> GetAllPeopleAsync()
    {
        var people = _persons.Values.AsEnumerable();

        return Task.FromResult(people);
    }

    public Task<Person> GetPersonByIdAsync(Guid id)
    {
        if (!_persons.TryGetValue(id, out Person found) || found is null)
        {
            throw new PersonNotFoundException($"Person not found with Id: {id}");
        }

        return Task.FromResult(found);
    }

    public Task<Person> UpdatePersonAsync(Guid id, Person person)
    {
        if (!_persons.TryGetValue(id, out Person found) || found is null)
        {
            throw new PersonNotFoundException($"Person not found with Id: {id}");
        }

        found.FirstName = person.FirstName;

        found.LastName = person.LastName;

        found.Age = person.Age;

        return Task.FromResult(found);
    }
}
