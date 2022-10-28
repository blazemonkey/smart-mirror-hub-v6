using Azure.Storage.Queues;
using Quartz;
using SmartMirrorHubV6.Updater.Models;
using System.Text.Json;

namespace SmartMirrorHubV6.Updater.Jobs;

public class AlexaQueueJob : BaseJob
{
    private readonly ILogger<AlexaQueueJob> _logger;

    public AlexaQueueJob(ILogger<AlexaQueueJob> logger, IConfiguration configuration, HttpClient httpClient) : base(configuration, httpClient)
    {
        _logger = logger;
    }

    public override async Task ExecuteJob(IJobExecutionContext context)
    {
        var queueUrl = Configuration["QueueUrl"];
        var queueClient = new QueueClient(queueUrl, "alexa-queue");
        var lastMessage = DateTime.MinValue;
        var client = new MirrorApiClient(ApiUrl, HttpClient);

        while (true)
        {
            var message = await queueClient.ReceiveMessageAsync();

            if (message != null && message.Value != null)
            {
                lastMessage = DateTime.Now;
                var messageValue = message.Value;

                try
                {
                    var queueMessage = await JsonSerializer.DeserializeAsync<QueueMessage>(messageValue.Body.ToStream());
                    _logger.LogInformation($"Received {queueMessage.ToggleType} {queueMessage.ComponentName} from {queueMessage.DeviceId}");

                    if (queueMessage.ToggleType == "get")
                        await client.RefreshMirrorComponentByVoiceAsync(queueMessage.DeviceId, queueMessage.ComponentName);
                    else
                        await client.ShowMirrorComponentByVoiceAsync(queueMessage.DeviceId, queueMessage.ToggleType, queueMessage.ComponentName);
                }
                catch (Exception) { }

                await queueClient.DeleteMessageAsync(messageValue.MessageId, messageValue.PopReceipt);
            }

            if (DateTime.Now.Subtract(lastMessage) > new System.TimeSpan(0, 0, 5, 0))
                Thread.Sleep(10 * 1000);
            else
                Thread.Sleep(1000);
        }
    }
}
