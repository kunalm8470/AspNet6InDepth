using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.v1.Persons.Commands.UpdatePerson;

public class UpdatePersonCommandHandler { }

//public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, Person>
//{
//    private readonly IPersonInMemoryRepository _repository;
//    private readonly IMapper _mapper;

//    public UpdatePersonCommandHandler(IPersonInMemoryRepository repository, IMapper mapper)
//    {
//        _repository = repository;

//        _mapper = mapper;
//    }

//    public async Task<Person> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
//    {
//        Person toUpdate = _mapper.Map<Person>(request);

//        Person updated = await _repository.UpdatePersonAsync(toUpdate.Id, toUpdate);

//        return updated;
//    }
//}
