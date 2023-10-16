using Api.Models;
using Api.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using static Api.Protos.PersonService;

namespace Api.Services;

public class PersonService : PersonServiceClient, IPersonService
{
    private readonly GrpcChannel _channel;

    private readonly PersonServiceClient _client;

    public PersonService(GrpcChannel channel)
    {
        _channel = channel;

        _client = new PersonServiceClient(_channel);
    }

    public async Task AddPersonAsync(string name, int age, CancellationToken cancellationToken = default)
    {
        await _client.AddPersonAsync(new Protos.PersonRequest
        {
            Name = name,
            Age = age
        }, cancellationToken: cancellationToken);
    }

    public async Task<List<Person>> GetPersonsPaginatedAsync(int page, int limit, CancellationToken cancellationToken = default)
    {
        List<Person> persons = new();

        AsyncServerStreamingCall<PersonResponse> call = _client.GetPersons(new Protos.PersonPaginationRequest
        {
            Page = page,
            Limit = limit
        }, cancellationToken: cancellationToken);

        await foreach (PersonResponse person in call.ResponseStream.ReadAllAsync(cancellationToken))
        {
            persons.Add(new Person
            {
                Id = Guid.Parse(person.Id),
                Name = person.Name,
                Age = person.Age,
                CreatedAt = person.CreatedAt.ToDateTime(),
                UpdatedAt = person.UpdatedAt is null ? null : person.UpdatedAt.ToDateTime()
            });
        }

        return persons;
    }

}
