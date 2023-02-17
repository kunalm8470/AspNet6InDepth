using Api.Models;

namespace Api.Interfaces
{
    public interface IJsonTypicodeClient
    {
        public Task<IEnumerable<Todo>> GetTodosAsync();
    }
}
