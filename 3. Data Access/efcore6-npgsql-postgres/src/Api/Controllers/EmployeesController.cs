using Api.Models.Dto.Responses;
using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Api.Models.Dto.Requests;
using Api.Interfaces;
using System.Globalization;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly HttpContext _context;

    private readonly IEmployeeRepository _repository;

    public EmployeesController(IEmployeeRepository repository,  IHttpContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor.HttpContext;
        
        _repository = repository;
    }

    [HttpGet("offset")]
    public async Task<ActionResult<IReadOnlyList<MappedEmployee>>> GetPersonsAsync([FromQuery] int page, [FromQuery] int limit)
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        IReadOnlyList<Employee> employees = await _repository.OffsetPaginationAsync(page, limit, requestCancellation);

        IReadOnlyList<MappedEmployee> mappedEmployees = employees.Select(e =>
        {
            MappedDepartment department = new(e.Department.Id, e.Department.Name);

            MappedEmployee employee = new(e.Id, e.FirstName, e.LastName, e.DisplayName, e.Email, e.Phone, e.Salary, e.HireDate.ToLongDateString(), department);

            return employee;
        })
        .ToList()
        .AsReadOnly();

        return Ok(mappedEmployees);
    }

    [HttpGet("keyset")]
    public async Task<IActionResult> GetPersonsAsync([FromQuery] string searchAfter, [FromQuery] int limit)
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        Guid? searchAfterId = default;
        DateTime? searchAfterDate = default;

        if (!string.IsNullOrEmpty(searchAfter) && !string.IsNullOrWhiteSpace(searchAfter))
        {
            searchAfterId = Guid.Parse(searchAfter.Split('_')[0]);

            searchAfterDate = DateTime.ParseExact(searchAfter.Split('_')[1], "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture);

            searchAfterDate = DateTime.SpecifyKind(searchAfterDate.Value, DateTimeKind.Utc);
        }

        IReadOnlyList<Employee> employees = await _repository.KeysetPaginationAsync(searchAfterId, searchAfterDate, limit, requestCancellation);

        IReadOnlyList<MappedEmployee> mappedEmployees = employees.Select(e =>
        {
            MappedDepartment department = new(e.Department.Id, e.Department.Name);

            MappedEmployee employee = new(e.Id, e.FirstName, e.LastName, e.DisplayName, e.Email, e.Phone, e.Salary, e.HireDate.ToLongDateString(), department);

            return employee;
        })
        .ToList()
        .AsReadOnly();


        string nextSearchAfter = null;

        if (employees.Any())
        {
            Employee lastEmployee = employees[^1];
            
            nextSearchAfter = $"{lastEmployee.Id}_{lastEmployee.CreatedAt:yyyy-MM-ddTHH:mm:ss}";
        }

        return Ok(new
        {
            searchAfter = nextSearchAfter,
            data = mappedEmployees
        });
    }

    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<MappedEmployee>> GetPersonsAsync([FromRoute] Guid id)
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        Employee found = await _repository.GetByIdAsync(id, requestCancellation);

        MappedDepartment department = new(found.Department.Id, found.Department.Name);

        MappedEmployee employee = new(found.Id, found.FirstName, found.LastName, found.DisplayName, found.Email, found.Phone, found.Salary, found.HireDate.ToLongDateString(), department);

        return Ok(employee);
    }

    [HttpPost]
    public async Task<ActionResult<Employee>> AddPersonAsync([FromBody] AddEmployee dto)
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        Employee employee = dto.ToEmployee();

        Employee created = await _repository.AddEmployeeAsync(employee, requestCancellation);

        return StatusCode(StatusCodes.Status201Created, created);
    }

    [HttpPut]
    public async Task<ActionResult<Employee>> UpdatePersonAsync([FromBody] Employee replacement)
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        Employee updated = await _repository.UpdateEmployeeAsync(replacement, requestCancellation);

        return Ok(updated);
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeletePersonAsync([FromRoute] Guid id)
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        await _repository.DeleteEmployeeAsync(id, requestCancellation);

        return NoContent();
    }
}
