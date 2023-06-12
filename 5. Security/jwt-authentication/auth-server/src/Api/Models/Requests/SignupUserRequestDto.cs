using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public class SignupUserRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 100 characters long.")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 100 characters long.")]
    public string LastName { get; set; }

    [Required]
    [RegularExpression("male|female", ErrorMessage = "Gender can be only \"male\" or \"female\".")]
    public string Gender { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Username should be between 2 and 50 characters long.")]
    public string Username { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 5, ErrorMessage = "Email should be between 5 and 256 characters long.")]
    public string Email { get; set; }

    [Required]
    [StringLength(1000, MinimumLength = 5, ErrorMessage = "Email should be between 5 and 1000 characters long.")]
    public string Password { get; set; }
}
