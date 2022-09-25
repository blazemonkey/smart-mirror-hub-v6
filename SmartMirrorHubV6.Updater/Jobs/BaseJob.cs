using Quartz;

namespace SmartMirrorHubV6.Updater.Jobs;

public abstract class BaseJob : IJob
{
    protected HttpClient HttpClient { get; private set; }
    protected IConfiguration Configuration { get; private set; }
    protected string ApiUrl { get; private set; }
    public BaseJob(IConfiguration configuration, HttpClient httpClient)
    {
        HttpClient = httpClient;
        Configuration = configuration;
        ApiUrl = Configuration["ApiUrl"];
    }

    public abstract Task ExecuteJob(IJobExecutionContext context);
    public Task Execute(IJobExecutionContext context)
    {
        return ExecuteJob(context);
    }
}
