using Api.Models;

namespace Api.Interfaces;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<Employee>> OffsetPaginationAsync(int page, int limit, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Employee>> KeysetPaginationAsync(Guid? searchAfterId, DateTime? searchAfterDate, int limit, CancellationToken cancellationToken = default);

    Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<Employee> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);

    Task<Employee> UpdateEmployeeAsync(Employee entity, CancellationToken cancellationToken = default);

    Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default);
}
