namespace Api.Models;

public class PersonAddress
{
    public Guid Id { get; set; }

    public Guid PersonId { get; set; }

    public string Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
