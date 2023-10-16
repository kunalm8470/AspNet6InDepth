namespace Api.Models;

public class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
