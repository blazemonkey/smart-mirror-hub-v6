using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json.Serialization;

namespace SmartMirrorHubV6.Shared.Components.Data.Network;

public class DeviceDetail
{
    [JsonPropertyName("deviceName")]

    public string DeviceName { get; set; }
    [JsonPropertyName("ownerName")]

    public string OwnerName { get; set; }
    [JsonPropertyName("ownerImageUrl")]

    public string OwnerImageUrl { get; set; }
    [JsonPropertyName("ownerType")]

    public DeviceOwnerType OwnerType { get; set; }
}

public enum DeviceOwnerType
{
    Resident = 1,
    Guest = 2,
    Pet = 3
}

public enum DeviceOwnerStatus
{
    Online = 1,
    Away = 2,
    Offline = 3
}

public class TpLinkDecoRecord
{
    public string OwnerName { get; set; }
    public string OwnerImageUrl { get; set; }
    public DateTime LastOnlineDateTimeUtc { get; set; }
    public bool ShowStatus { get; set; }
    public DeviceOwnerStatus OwnerStatus { get; set; }
}

public class TpLinkDecoResponse : ComponentResponse
{
    public DateTime DateTimeUtc { get; set; }
    public TpLinkDecoRecord[] Records { get; set; }
}
