using Api.Common.JsonConverters;
using Api.Common.ValidationAttributes;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Dto.Requests;

public class AddEmployee
{
    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "First Name field should be between 1 and 200 characters long.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Last Name field should be between 1 and 200 characters long.")]
    public string LastName { get; set; }

    [Required]
    [StringLength(256, ErrorMessage = "Invalid email format.")]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [RegularExpression(@"^\+[1-9]\d{10,14}$", ErrorMessage = "Phone number field must be in E.164 format.")]
    public string Phone { get; set; }

    [Required]
    public decimal Salary { get; set; }

    [Required]
    [JsonConverter(typeof(DateOnlyJsonConverter))]
    public DateOnly HireDate { get; set; }

    [Required]
    [NotEmptyGuid(ErrorMessage = "Department Id field cannot be an empty UUIDv4.")]
    public Guid DepartmentId { get; set; }

    /// <summary>
    /// Mapping method to employee object
    /// </summary>
    /// <returns></returns>
    public Employee ToEmployee()
    {
        return new Employee
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Phone = Phone,
            Salary = Salary,
            HireDate = HireDate,
            DepartmentId = DepartmentId
        };
    }
}
