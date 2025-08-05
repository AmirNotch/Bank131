namespace BackgroundBank131Service.Background;
using BackgroundBank131Service.Services;
using static BackgroundBank131Service.Models.Constants;

public class SendingPayoutSessionJob : AbstractJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SendingPayoutSessionJob(IServiceScopeFactory serviceScopeFactory, ILogger<SendingPayoutSessionJob> logger)
        : base(logger, nameof(SendingPayoutSessionJob), jobWaitingTimeMs: SendingPayoutSessionJobWaitingTimeInMs)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        try        
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var processingService = scope.ServiceProvider.GetRequiredService<SessionProcessingService>();
                await processingService.ProcessPendingSessionsConfirmOrCancel(stoppingToken);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{name} encountered error during execution.", _className);
            throw;
        }
    }
}