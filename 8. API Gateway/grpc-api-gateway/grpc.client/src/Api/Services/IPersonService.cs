using Api.Models;

namespace Api.Services;

public interface IPersonService
{
    public Task<List<Person>> GetPersonsPaginatedAsync(int page, int limit, CancellationToken cancellationToken = default(CancellationToken));

    public Task AddPersonAsync(string name, int age, CancellationToken cancellationToken = default);
}
