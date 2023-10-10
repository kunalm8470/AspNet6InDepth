using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public class AddPersonDto
{
    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "First name should be between 1 and 200 characters")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Last name should be between 1 and 200 characters")]
    public string LastName { get; set; }

    [Required]
    public int Age { get; set; }
}
