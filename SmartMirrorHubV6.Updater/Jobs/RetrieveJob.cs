using Quartz;

namespace SmartMirrorHubV6.Updater.Jobs;

[DisallowConcurrentExecution]
public class RetrieveJob : BaseJob
{
    private readonly ILogger<RetrieveJob> _logger;
    public RetrieveJob(ILogger<RetrieveJob> logger, HttpClient httpClient, IConfiguration configuration) : base(configuration, httpClient)
    {
        _logger = logger;
    }
    public override async Task ExecuteJob(IJobExecutionContext context)
    {
        try
        {
            _logger.LogInformation($"Connecting to {ApiUrl}");
            var client = new MirrorApiClient(ApiUrl, HttpClient);
            var mirrors = await client.GetAlMirrorsAsync(true, true);
            _logger.LogInformation($"Found {mirrors.Count} mirrors");

            var components = await client.GetAllComponentsAsync();
            _logger.LogInformation($"Found {components.Count} components");

            var tasks = new List<Task<ComponentResponse>>();
            foreach (var m in mirrors)
            {
                if (m.Live == false)
                {
                    _logger.LogInformation($"Skipping '{m.Name}' mirror because it is not live");
                    continue;
                }

                _logger.LogInformation($"Found {m.MirrorComponents.Count} components for '{m.Name}'");
                foreach (var mc in m.MirrorComponents)
                {
                    if (mc.Active == false)
                    {
                        _logger.LogInformation($"Skipping '{mc.Name}' component because it is inactive");
                        continue;
                    }

                    var component = components.FirstOrDefault(x => x.Id == mc.ComponentId);
                    if (component == null)
                    {
                        _logger.LogInformation($"Skipping '{mc.Name}' component because ComponentId '{mc.ComponentId}' is invalid");
                        continue;
                    }

                    if (component.Interval == 0)
                    {
                        _logger.LogInformation($"Skipping '{mc.Name}' component because component '{component.Name} does not run on a schedule");
                        continue;
                    }

                    if (mc.InSchedule == false)
                    {
                        _logger.LogInformation($"Skipping '{mc.Name}' component because it is not in schedule");
                        continue;
                    }

                    if (mc.LastUpdatedTimeUtc.DateTime != DateTime.MinValue && DateTime.UtcNow.Subtract(mc.LastUpdatedTimeUtc.DateTime).TotalSeconds < (component.Interval >= 60 ? (component.Interval - 10) : component.Interval)) // 10 second buffer
                    {
                        _logger.LogInformation($"Skipping '{mc.Name}' component because it does not need to be run yet");
                        continue;
                    }

                    _logger.LogInformation($"Retrieving '{mc.Name}' component");
                    var task = client.RetrieveMirrorComponentAsync(mc.Id);
                    tasks.Add(task);
                }
            }

            await Task.WhenAll(tasks);
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"An unexpected error has occured: {ex}");
        }
    }
}
