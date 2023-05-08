using Api.Common.Exceptions;
using Api.Interfaces;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly CompanyDbContext _context;

    public EmployeeRepository(CompanyDbContext context)
    {
        _context = context;
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
    {
        _context.Employees.Add(employee);

        await _context.SaveChangesAsync(cancellationToken);

        return employee;
    }

    public async Task DeleteEmployeeAsync(Guid id, CancellationToken cancellationToken = default)
    {
        Employee found = await GetByIdAsync(id, cancellationToken);

        if (found is null)
        {
            throw new NotFoundException($"Employee not found with id: {id}");
        }

        _context.Employees.Remove(found);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<Employee> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // No Eager loading
        // Employee found = await _context.Employees.FindAsync(id, cancellationToken);

        // Eager loading
        return await _context.Employees
        .Include(e => e.Department)
        .SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Employee>> KeysetPaginationAsync(Guid? searchAfterId, DateTime? searchAfterDate, int limit, CancellationToken cancellationToken = default)
    {
        var query = _context.Employees
        .Include(e => e.Department) // Eager loading
        .OrderByDescending(e => e.CreatedAt)
        .ThenByDescending(e => e.Id);

        if (searchAfterId is not null && searchAfterDate is not null)
        {
            return (
                await query.Where(e =>
                e.CreatedAt.ToUniversalTime() < searchAfterDate.Value
                || (e.CreatedAt.ToUniversalTime() == searchAfterDate.Value && e.Id.CompareTo(searchAfterId.Value) < 0)
            )
            .Take(limit)
            .ToListAsync(cancellationToken))
            .AsReadOnly();
        }

        return (
            await query
            .Take(limit)
            .ToListAsync(cancellationToken)
        ).AsReadOnly();
    }

    public async Task<IReadOnlyList<Employee>> OffsetPaginationAsync(int page, int limit, CancellationToken cancellationToken = default)
    {
        int skip = (page - 1) * limit;

        return await _context.Employees
        .Include(e => e.Department) // Eager loading
        .OrderByDescending(e => e.CreatedAt)
        .ThenByDescending(e => e.Id)
        .Skip(skip)
        .Take(limit)
        .ToListAsync(cancellationToken);
    }

    public async Task<Employee> UpdateEmployeeAsync(Employee entity, CancellationToken cancellationToken = default)
    {
        Employee found = await GetByIdAsync(entity.Id, cancellationToken);

        if (found is null)
        {
            throw new NotFoundException($"Employee not found with id: {entity.Id}");
        }

        // Update the employee in-memory
        found.FirstName = entity.FirstName;

        found.LastName = entity.LastName;

        found.Email = entity.Email;

        found.Phone = entity.Phone;

        found.HireDate = entity.HireDate;

        found.Salary = entity.Salary;

        found.UpdatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);

        _context.Employees.Update(found);

        await _context.SaveChangesAsync(cancellationToken);

        return found;
    }
}
