using Api.Interfaces.ThirdParty;
using Api.Models;
using Newtonsoft.Json;

namespace Api.HttpClients;

public class UserClient : IUserClient
{
    private readonly IHttpClientFactory _factory;

    public UserClient(IHttpClientFactory factory)
	{
        _factory = factory;
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        HttpClient client = _factory.CreateClient();

        HttpResponseMessage response = await client.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");

        response.EnsureSuccessStatusCode();

        string serializedResponse = await response.Content.ReadAsStringAsync();

        User foundUser = JsonConvert.DeserializeObject<User>(serializedResponse);

        return foundUser;
    }
}
