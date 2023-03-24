namespace Api.Services;

public class IndependentBackgroundService : BackgroundService
{
    private readonly ILogger<IndependentBackgroundService> _logger;

    public IndependentBackgroundService(ILogger<IndependentBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(5000, stoppingToken);

            _logger.LogInformation("Independent background service running on : {datetime}", DateTime.Now);
        }
    }
}
