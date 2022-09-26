using Quartz;

namespace SmartMirrorHubV6.Updater.Jobs;

[DisallowConcurrentExecution]
public class CleanJob : BaseJob
{
    private readonly ILogger<CleanJob> _logger;
    public CleanJob(ILogger<CleanJob> logger, HttpClient httpClient, IConfiguration configuration) : base(configuration, httpClient)
    {
        _logger = logger;
    }

    public override async Task ExecuteJob(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation($"Connecting to {ApiUrl}");
            var client = new MirrorApiClient(ApiUrl, HttpClient);
            await client.PruneResponseHistoryAsync(null, DateTime.UtcNow.Date.AddDays(-7));
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"An unexpected error has occured: {ex}");
        }
    }
}

