using Quartz;
using SmartMirrorHubV6.Updater.Jobs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();

    //var retrieveJob = new JobKey(nameof(RetrieveJob));
    //q.AddJob<RetrieveJob>(o => o.WithIdentity(retrieveJob));
    //q.AddTrigger(o => o
    //    .ForJob(retrieveJob)
    //    .WithIdentity($"{nameof(RetrieveJob)}-Trigger")
    //    .WithCronSchedule("0 * * * * ?"));

    //var updateJob = new JobKey(nameof(UpdateJob));
    //q.AddJob<UpdateJob>(o => o.WithIdentity(updateJob));
    //q.AddTrigger(o => o
    //    .ForJob(updateJob)
    //    .WithIdentity($"{nameof(UpdateJob)}-Trigger")
    //    .WithCronSchedule("1 * * * * ?"));

    //var cleanJob = new JobKey(nameof(CleanJob));
    //q.AddJob<CleanJob>(o => o.WithIdentity(cleanJob));
    //q.AddTrigger(o => o
    //    .ForJob(cleanJob)
    //    .WithIdentity($"{nameof(CleanJob)}-Trigger")
    //    .WithCronSchedule("0 0 0 * * ?"));


    var queueJob = new JobKey(nameof(AlexaQueueJob));
    q.AddJob<AlexaQueueJob>(o => o.WithIdentity(queueJob));
    q.AddTrigger(o => o
        .ForJob(queueJob)
        .WithIdentity($"{nameof(AlexaQueueJob)}-Trigger")
        .StartNow());
});

builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true);

builder.Services.AddHttpClient();
var app = builder.Build();

app.Run();
