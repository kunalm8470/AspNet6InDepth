using Api.Models;

namespace Api.Interfaces;

public interface IPersonsRepository
{
    public Task<IReadOnlyList<Person>> GetPersonOffsetPaginationAsync(int page, int limit, CancellationToken cancellationToken);

    public Task<IReadOnlyList<Person>> GetPersonKeysetPaginationAsync(Guid? searchAfter, DateTime? searchAfterTime, int limit, CancellationToken cancellationToken);

    public Task<Person> GetPersonByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<Person> AddPersonAsync(Person person, CancellationToken cancellationToken);

    public Task DeletePersonAsync(Guid id, CancellationToken cancellationToken);

    public Task AddAddressToPersonAsync(Guid id, string address, CancellationToken cancellationToken);
}
