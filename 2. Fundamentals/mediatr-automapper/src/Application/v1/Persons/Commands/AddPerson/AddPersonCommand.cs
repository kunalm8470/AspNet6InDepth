using MediatR;

namespace Application.v1.Persons.Commands.AddPerson;

/*
    To make a command, we need to implement IRequest<T> interface
    where T is output of the command
*/
public class AddPersonCommand : IRequest<Unit>
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public int Age { get; set; }
}
