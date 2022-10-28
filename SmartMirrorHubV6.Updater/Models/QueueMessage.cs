using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Updater.Models;

public class QueueMessage
{
    [JsonPropertyName("toggleType")]
    public string ToggleType { get; set; }
    [JsonPropertyName("componentName")]
    public string ComponentName { get; set; }
    [JsonPropertyName("deviceId")]
    public string DeviceId { get; set; }
}
