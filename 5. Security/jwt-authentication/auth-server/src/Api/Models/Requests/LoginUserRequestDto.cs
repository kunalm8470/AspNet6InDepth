using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public class LoginUserRequestDto
{
	[Required]
    public string Username { get; set; }

	[Required]
    public string Password { get; set; }
	
	[JsonProperty("grant_type")]
	[Required]
	[RegularExpression("password", ErrorMessage = "Invalid grant type")]
	public string GrantType { get; set; }
}
