using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public class RefreshTokenRequestDto
{
    [JsonProperty("refresh_token")]
    [Required]
    public string RefreshToken { get; set; }

    [JsonProperty("grant_type")]
    [Required]
    [RegularExpression("refresh_token", ErrorMessage = "Invalid grant type, supported grant types are \"refresh_token\"")]
    public string GrantType { get; set; }
}
