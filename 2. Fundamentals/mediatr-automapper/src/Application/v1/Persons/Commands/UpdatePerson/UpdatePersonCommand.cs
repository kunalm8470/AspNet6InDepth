using Domain.Entities;
using MediatR;

namespace Application.v1.Persons.Commands.UpdatePerson;

public class UpdatePersonCommand : IRequest<Person>
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int Age { get; set; }
}
