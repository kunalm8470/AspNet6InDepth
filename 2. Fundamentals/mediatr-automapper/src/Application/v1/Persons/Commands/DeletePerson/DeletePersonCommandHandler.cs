using Domain.Interfaces;
using MediatR;

namespace Application.v1.Persons.Commands.DeletePerson;

public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, Unit>
{
    private readonly IInMemoryPersonRepository _repository;

    public DeletePersonCommandHandler(IInMemoryPersonRepository repository)
	{
        _repository = repository;
    }

    public async Task<Unit> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeletePersonAsync(request.Id);

        return Unit.Value;
    }
}
