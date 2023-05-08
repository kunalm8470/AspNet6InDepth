using Api.Models;

namespace Api.Interfaces;

public interface IDepartmentRepository
{
    Task<IReadOnlyList<Department>> GetDepartmentsAsync(CancellationToken cancellationToken = default);
}
