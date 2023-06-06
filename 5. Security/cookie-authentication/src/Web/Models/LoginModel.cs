using System.ComponentModel.DataAnnotations;

namespace Web.Models;

public class LoginModel
{
    [Required]
    [MinLength(10)]
    [MaxLength(256)]
    public string Email { get; set; }

    [Required]
    [MinLength(1)]
    [MaxLength(500)]
    public string Password { get; set; }


    public string ReturnUrl { get; set; }
}
