using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.v1.Persons.Commands.AddPerson;

/*
    Commands will be processed by a CommandHandler

    To make a commandhandler we will implement IRequestHandler<CommandType, Output>
*/
public class AddPersonCommandHandler : IRequestHandler<AddPersonCommand, Unit>
{
    private readonly IInMemoryPersonRepository _repository;
    private readonly IMapper _mapper;

    public AddPersonCommandHandler(IInMemoryPersonRepository repository, IMapper mapper)
    {
        _repository = repository;
        
        _mapper = mapper;
    }

    public async Task<Unit> Handle(AddPersonCommand request, CancellationToken cancellationToken)
    {
        Person newPerson = _mapper.Map<Person>(request);

        await _repository.AddPersonAsync(newPerson);

        return Unit.Value;
    }
}
