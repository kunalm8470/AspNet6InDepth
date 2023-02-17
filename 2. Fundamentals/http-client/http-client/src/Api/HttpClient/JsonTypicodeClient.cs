using Api.Interfaces;
using Api.Models;
using Newtonsoft.Json;

namespace Api.HttpClient
{
    public class JsonTypicodeClient : IJsonTypicodeClient
    {
        private readonly System.Net.Http.HttpClient _client;

        public JsonTypicodeClient(System.Net.Http.HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<Todo>> GetTodosAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("/todos");

                string serializedResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<IEnumerable<Todo>>(serializedResponse);
            }
            catch (HttpRequestException ex) // Something went wrong while request is being sent
            {
                return null;
            }
        }
    }
}
