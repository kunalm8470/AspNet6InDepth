using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public class AddPersonAddressDto
{
    [Required]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Address must be between 1 to 500 characters in length.")]
    public string Address { get; set; }
}
