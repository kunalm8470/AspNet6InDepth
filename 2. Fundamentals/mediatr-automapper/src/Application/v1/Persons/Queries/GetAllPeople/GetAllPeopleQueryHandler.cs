using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.v1.Persons.Queries.GetAllPeople;

public class GetAllPeopleQueryHandler : IRequestHandler<GetAllPeopleQuery, IEnumerable<Person>>
{
    private readonly IInMemoryPersonRepository _repository;

    public GetAllPeopleQueryHandler(IInMemoryPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Person>> Handle(GetAllPeopleQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllPeopleAsync();
    }
}
