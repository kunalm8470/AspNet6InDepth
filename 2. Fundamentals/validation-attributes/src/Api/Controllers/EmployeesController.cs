using Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{

    [HttpPost]
    public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
    {
        return Ok(employee);
    }
}
