using Quartz;
using SmartMirrorHubV6.Updater.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    var updateJob = new JobKey(nameof(UpdateJob));
    q.AddJob<UpdateJob>(o => o.WithIdentity(updateJob));
    q.AddTrigger(o => o
        .ForJob(updateJob)
        .WithIdentity($"{nameof(UpdateJob)}-Trigger")
        .WithCronSchedule("0 * * * * ?"));

    var cleanJob = new JobKey(nameof(CleanJob));
    q.AddJob<CleanJob>(o => o.WithIdentity(cleanJob));
    q.AddTrigger(o => o
        .ForJob(cleanJob)
        .WithIdentity($"{nameof(CleanJob)}-Trigger")
        .WithCronSchedule("0 0 0 * * ?"));
});

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

builder.Services.AddHttpClient();
var app = builder.Build();

app.Run();
