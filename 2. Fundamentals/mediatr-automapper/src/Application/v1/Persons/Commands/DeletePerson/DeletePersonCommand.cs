using MediatR;

namespace Application.v1.Persons.Commands.DeletePerson;

public class DeletePersonCommand : IRequest<Unit>
{
    public Guid Id { get; set; }
}
