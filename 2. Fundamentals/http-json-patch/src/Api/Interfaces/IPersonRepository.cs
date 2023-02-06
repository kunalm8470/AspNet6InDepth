using Api.Models;

namespace Api.Interfaces;

public interface IPersonRepository
{
    public Task<IEnumerable<Person>> GetPersonsPaginatedAsync(int page, int limit);

    public Task<Person> GetPersonByIdAsync(Guid id);

    public Task<Person> UpdatePersonAsync(Guid id, Person replacement);
}
