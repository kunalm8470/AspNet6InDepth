using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Api.Models.Requests;

public class RevokeRefreshTokenRequestDto
{
    [Required]
    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }
}
