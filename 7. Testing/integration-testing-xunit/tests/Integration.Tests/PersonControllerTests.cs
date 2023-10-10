using Api.Models;
using Api.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Text;

namespace Integration.Tests;

public class PersonControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    private readonly HttpClient _client;

    public PersonControllerTests(CustomWebApplicationFactory factory)
	{
        _factory = factory;

        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 10)]
    public async Task GetPersonsAsync_WhenCalledWithValidPageAndLimit_ReturnsHttp200OkWithListOfPersonsInResponseBody(int page, int limit)
    {
        // Arrange
        Dictionary<string, string> queryParameters = new()
        {
            ["page"] = page.ToString(),
            ["limit"] = limit.ToString()
        };

        string uri = QueryHelpers.AddQueryString("/api/Persons/offset", queryParameters);

        // Act
        HttpResponseMessage response = await _client.GetAsync(uri);

        // Assert
        Assert.Equal(StatusCodes.Status200OK, (int)response.StatusCode);

        string responsePayload = await response.Content.ReadAsStringAsync();

        Assert.NotNull(responsePayload);

        List<Person> persons = JsonConvert.DeserializeObject<List<Person>>(responsePayload);

        Assert.NotNull(persons);
    }

    [Fact]
    public async Task AddPersonAsync_WhenCalledWithValidParameters_ReturnsHttp201Created()
    {
        // Arrange
        AddPersonDto dto = new()
        {
            FirstName = "JJ",
            LastName = "Smith",
            Age = 20
        };

        StringContent content = new(
            content: JsonConvert.SerializeObject(dto),
            encoding: Encoding.UTF8,
            mediaType: "application/json"
        );

        string uri = "/api/Persons";

        // Act
        HttpResponseMessage response = await _client.PostAsync(uri, content);

        // Assert
        Assert.Equal(StatusCodes.Status201Created, (int)response.StatusCode);
    }
}