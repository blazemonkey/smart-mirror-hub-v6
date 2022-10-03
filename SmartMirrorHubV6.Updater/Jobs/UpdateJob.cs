using Quartz;

namespace SmartMirrorHubV6.Updater.Jobs;

[DisallowConcurrentExecution]
public class UpdateJob : BaseJob
{
    private readonly ILogger<UpdateJob> _logger;
    public UpdateJob(ILogger<UpdateJob> logger, HttpClient httpClient, IConfiguration configuration) : base(configuration, httpClient)
    {
        _logger = logger;
    }
    public override async Task ExecuteJob(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation($"Connecting to {ApiUrl}");
            var client = new MirrorApiClient(ApiUrl, HttpClient);
            var mirrors = await client.GetAlMirrorsAsync(true);
            _logger.LogInformation($"Found {mirrors.Count} mirrors");

            var tasks = new List<Task>();
            foreach (var mirror in mirrors)
            {
                var task = client.RefreshMirrorComponentsByMirrorIdAsync(mirror.Id);
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"An unexpected error has occured: {ex}");
        }
    }
}