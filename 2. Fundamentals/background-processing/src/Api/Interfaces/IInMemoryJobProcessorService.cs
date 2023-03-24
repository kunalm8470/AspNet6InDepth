namespace Api.Interfaces;

public interface IInMemoryJobProcessorService
{
    Task SendMessageAsync(string message, CancellationToken token = default);
}
