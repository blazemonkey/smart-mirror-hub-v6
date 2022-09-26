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

                    if (ShowMirrorComponent(mc, m) == false)
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

    private bool ShowMirrorComponent(MirrorComponent mirrorComponent, Mirror mirror)
    {
        var mirrorSchedule = mirror.Schedule;
        var mirrorComponentSchedule = mirrorComponent.Schedule;

        if (mirrorSchedule.Length != mirrorComponentSchedule.Length)
            return false;

        try
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById(mirror.Timezone);
            var time = TimeZoneInfo.ConvertTime(DateTime.Now, timezone);

            var component = mirrorComponentSchedule.Substring((int)time.DayOfWeek * 96, 96); // day
            component = component.Substring(time.Hour * 4, 4); // hour

            var on = component.Substring(time.Minute / 15, 1); // 15-minute block
            if (on == "0")
                return false;

            var global = mirror.Schedule.Substring((int)time.DayOfWeek * 96, 96); // day
            global = global.Substring(time.Hour * 4, 4); // hour

            var globalOn = component.Substring(time.Minute / 15, 1); // 15-minute block
            return globalOn == "1";
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
