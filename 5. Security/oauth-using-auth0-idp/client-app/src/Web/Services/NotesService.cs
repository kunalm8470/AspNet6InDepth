using Newtonsoft.Json;
using Web.Models;

namespace Web.Services;

public class NotesService : INotesService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NotesService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IReadOnlyList<Note>> GetNotesAsync()
    {
        using HttpClient client = _httpClientFactory.CreateClient("notes-api");

        HttpResponseMessage response = await client.GetAsync("/api/Notes");

        // Throw a HttpResponseException if status code is not 200-300 series
        response.EnsureSuccessStatusCode();

        string serializedResponse = await response.Content.ReadAsStringAsync();

        IReadOnlyList<Note> notes = JsonConvert.DeserializeObject<IReadOnlyList<Note>>(serializedResponse);

        return notes;
    }
}
