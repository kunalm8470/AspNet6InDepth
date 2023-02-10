using Api.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IValidator<Employee> _employeeValidator;

    public EmployeesController(IValidator<Employee> employeeValidator)
    {
        _employeeValidator = employeeValidator;
    }

    [HttpPost]
    public ActionResult<Employee> CreateEmployee([FromBody] Employee employee)
    {
        // Perform manual validation as suggested by FluentValidation library
        ValidationResult result = _employeeValidator.Validate(employee);

        if (!result.IsValid)
        {
            // Copy the validation results into ModelState.
            // ASP.NET 6 uses the ModelState collection to populate 
            // error messages to the API.
            result.AddToModelState(ModelState);

            // re-render the view when validation failed.
            return ValidationProblem(ModelState);
        }


        // Do some logic here to persist data

        return Ok(employee);
    }
}
