namespace Api.Models.Entities;

public class RefreshToken
{
    public Guid UserId { get; set; }

    public string Token { get; set; }
}
