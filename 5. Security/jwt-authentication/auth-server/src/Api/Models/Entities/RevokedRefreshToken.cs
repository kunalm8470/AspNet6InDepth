namespace Api.Models.Entities;

public class RevokedRefreshToken
{
    public Guid UserId { get; set; }

    public string Token { get; set; }
}
