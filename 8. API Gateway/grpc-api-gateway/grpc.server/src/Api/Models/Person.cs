using Bogus;

namespace Api.Models;

public class Person
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}

public static class PersonSeeder
{
    public static IEnumerable<Person> GenerateRandomPersons(int count)
    {
        return Enumerable.Range(0, count)
        .Select(_ =>
        {
            return new Faker<Person>()
            .RuleFor(p => p.Id, f => Guid.NewGuid())
            .RuleFor(p => p.Name, f => $"{f.Name.FirstName()} {f.Name.LastName()}")
            .RuleFor(p => p.Age, f => f.Random.Int(20, 50))
            .RuleFor(p => p.CreatedAt, f => DateTime.UtcNow)
            .RuleFor(p => p.UpdatedAt, f => default(DateTime?))
            .Generate();
        });
    }
}