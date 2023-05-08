using Api.Models.Dto.Responses;

namespace Api.Models;

public class Employee : BaseEntity<Guid>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string DisplayName { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public decimal Salary { get; set; }

    public DateOnly HireDate { get; set; }

    /// <summary>
    /// Foreign key for employee's department
    /// </summary>
    public Guid DepartmentId { get; set; }


    /// <summary>
    /// Shadow property for doing .Include using EF Core 6
    /// 
    /// This property will be populated and the department belonging to
    /// particular employee will be filled.
    /// </summary>
    public Department Department { get; set; } = null;


    public MappedEmployee ToMappedEmployee()
    {
        MappedDepartment belongsToDepartment = new(DepartmentId, null);

        if (Department is not null)
        {
            belongsToDepartment = new(Department.Id, Department.Name);
        }

        MappedEmployee employee = new(Id, FirstName, LastName, DisplayName, Email, Phone, Salary, HireDate.ToShortDateString(), belongsToDepartment);

        return employee;
    }
}
