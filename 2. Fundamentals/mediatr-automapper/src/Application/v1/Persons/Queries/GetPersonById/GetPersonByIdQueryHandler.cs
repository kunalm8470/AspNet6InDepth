using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.v1.Persons.Queries.GetPersonById;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Person>
{
    private readonly IInMemoryPersonRepository _repository;

    public GetPersonByIdQueryHandler(IInMemoryPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<Person> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetPersonByIdAsync(request.Id);
    }
}
