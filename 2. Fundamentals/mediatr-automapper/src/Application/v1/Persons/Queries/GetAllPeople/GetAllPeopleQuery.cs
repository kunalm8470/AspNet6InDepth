using Domain.Entities;
using MediatR;

namespace Application.v1.Persons.Queries.GetAllPeople;

public class GetAllPeopleQuery : IRequest<IEnumerable<Person>>
{

}
