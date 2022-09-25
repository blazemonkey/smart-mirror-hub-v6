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
});

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

builder.Services.AddHttpClient();
var app = builder.Build();

app.Run();
