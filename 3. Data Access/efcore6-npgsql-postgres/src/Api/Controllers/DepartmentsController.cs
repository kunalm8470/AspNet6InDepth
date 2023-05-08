using Api.Interfaces;
using Api.Models;
using Api.Models.Dto.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentRepository _repository;

    private readonly HttpContext _context;

    public DepartmentsController(IDepartmentRepository repository, IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;

        _context = httpContextAccessor.HttpContext;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<MappedDepartment>>> GetDepartmentsAsync()
    {
        CancellationToken requestCancellation = _context.RequestAborted;

        IReadOnlyList<Department> departments = await _repository.GetDepartmentsAsync(requestCancellation);

        IReadOnlyList<MappedDepartment> mappedDepartments = departments.Select(d =>
        {
            return new MappedDepartment(d.Id, d.Name);
        }).ToList().AsReadOnly();

        return Ok(mappedDepartments);
    }
}
