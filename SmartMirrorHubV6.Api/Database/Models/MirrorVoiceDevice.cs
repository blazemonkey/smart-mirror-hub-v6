namespace SmartMirrorHubV6.Api.Database.Models;

public class MirrorVoiceDevice : BaseModel
{
    public int MirrorId { get; set; }
    public VoiceDeviceType Type { get; set; }
    public string DeviceId { get; set; }
}

public enum VoiceDeviceType
{
    Alexa = 1,
    Siri = 2
}
