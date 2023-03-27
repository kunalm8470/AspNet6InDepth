using Domain.Entities;
using MediatR;

namespace Application.v1.Persons.Queries.GetPersonById;

public class GetPersonByIdQuery : IRequest<Person>
{
    public Guid Id { get; set; }
}
