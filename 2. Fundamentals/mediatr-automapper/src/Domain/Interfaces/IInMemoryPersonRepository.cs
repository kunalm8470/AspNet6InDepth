using Domain.Entities;

namespace Domain.Interfaces;

public interface IInMemoryPersonRepository
{
    Task<IEnumerable<Person>> GetAllPeopleAsync();

    Task<Person> GetPersonByIdAsync(Guid id);

    Task AddPersonAsync(Person person);

    Task<Person> UpdatePersonAsync(Guid id, Person person);

    Task DeletePersonAsync(Guid id);
}
