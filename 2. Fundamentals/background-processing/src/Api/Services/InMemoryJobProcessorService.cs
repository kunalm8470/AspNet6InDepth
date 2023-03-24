using Api.Interfaces;
using System.Threading.Channels;

namespace Api.Services;

public class InMemoryJobProcessorService : BackgroundService, IInMemoryJobProcessorService
{
    private readonly ChannelWriter<string> _channelWriter;

    private readonly ChannelReader<string> _channelReader;
    
    private readonly ILogger<InMemoryJobProcessorService> _logger;

    public InMemoryJobProcessorService(
        ChannelWriter<string> channelWriter,
        ChannelReader<string> channelReader,
        ILogger<InMemoryJobProcessorService> logger
    )
    {
        _channelWriter = channelWriter;
        
        _channelReader = channelReader;
        
        _logger = logger;
    }

    public async Task SendMessageAsync(string message, CancellationToken token = default)
    {
        await _channelWriter.WriteAsync(message, token);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting the job service");

        // Read the messages one-by-one from the channel using ChannelReader
        await foreach (string message in _channelReader.ReadAllAsync(stoppingToken))
        {
            _logger.LogInformation("Recieved message: {message}", message);
        }

        _logger.LogInformation("Stopping the job service");
    }
}
