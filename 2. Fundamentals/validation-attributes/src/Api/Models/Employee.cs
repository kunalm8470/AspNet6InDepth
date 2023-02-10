using Api.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class Employee
{
    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 2, ErrorMessage = "First name should be more than 2 characters and 200 characters")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(maximumLength: 200, MinimumLength = 1, ErrorMessage = "Last name should be more than 2 characters and 200 characters")]
    public string LastName { get; set; }

    [Required]
    [PreviousDateValidation(ErrorMessage = "Date of birth should be less than today's date")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public DateTime DateOfJoining { get; set; }

    [Required]
    [RegularExpression(pattern: "EMP\\d{5}", ErrorMessage = "Invalid Employee Code")]
    public string Code { get; set; }

    /// <summary>
    /// Need not do any validations for Enums
    /// </summary>
    [Required]
    public Gender Gender { get; set; }

    [StringLength(maximumLength: 200, MinimumLength = 1, ErrorMessage = "Designation should me more than 2 characters and 200 characters")]
    [Required]
    public string Designation { get; set; }

    [EmailAddress]
    [Required]
    public string Email { get; set; }
}

public enum Gender
{
    Male = 0,
    Female,
    Others
}
