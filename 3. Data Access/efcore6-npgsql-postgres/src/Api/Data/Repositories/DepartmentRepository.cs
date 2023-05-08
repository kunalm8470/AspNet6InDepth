using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repositories;

public class DepartmentRepository : IDepartmentRepository
{
    private readonly CompanyDbContext _context;

    public DepartmentRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Department>> GetDepartmentsAsync(CancellationToken cancellationToken = default)
    {
        return (await _context.Departments.ToListAsync(cancellationToken)).AsReadOnly();
    }
}
