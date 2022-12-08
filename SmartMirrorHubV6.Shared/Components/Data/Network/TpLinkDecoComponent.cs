using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System.Text.Json;

namespace SmartMirrorHubV6.Shared.Components.Data.Network;

public class TpLinkDecoComponent : DockerComponent
{
    public override string Name => "Home Online";
    public override string Description => "Get who's online at home";
    public override string Author => "TP Link Deco";
    public override ComponentCategory Category => ComponentCategory.Network;
    public override int Interval => 60;
    public override string VoiceName => "Who's Online";

    #region Inputs
    [ComponentInput("Devices JSON")]
    public DeviceDetail[] Devices { get; set; }

    /// <summary>
    /// Self dependency because it needs it's own components history
    /// </summary>
    [ComponentDepends("Self Dependency Component", false)]
    public TpLinkDecoResponse[] SelfDependencyComponent { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
#if DEBUG
        OutputDirectory = "L:\\Programming\\Projects\\TPLinkScraper\\output";
        OutputFilename = "tp-link-scraper-devices.json";
#endif

        var fileToOpen = Path.Combine(OutputDirectory, OutputFilename);
        var lastUpdatedTime = File.GetLastWriteTimeUtc(fileToOpen);
        if (DateTime.UtcNow.Subtract(lastUpdatedTime).TotalSeconds > Interval)
            return new ComponentResponse() { Error = "File was updated too long ago" };

        using var fileStream = File.Open(fileToOpen, FileMode.Open);
        var devices = await JsonSerializer.DeserializeAsync<string[]>(fileStream);

        var records = new List<TpLinkDecoRecord>();
        foreach (var d in Devices.OrderBy(x => x.OwnerType))
        {
            var record = new TpLinkDecoRecord()
            {
                OwnerName = d.OwnerName,
                OwnerImageUrl = d.OwnerImageUrl,
                ShowStatus = d.OwnerType == DeviceOwnerType.Resident
            };

            if (d.OwnerType == DeviceOwnerType.Resident)
            {
                var lastOnlineTimeUtc = DateTime.MinValue;
                if (devices.Any(x => x.ToLower() == d.DeviceName.ToLower()))
                    lastOnlineTimeUtc = lastUpdatedTime;
                else
                {
                    foreach (var sdc in SelfDependencyComponent)
                    {
                        var sdcRecord = sdc.Records.FirstOrDefault(x => x.OwnerName == d.OwnerName);
                        if (sdcRecord != null)
                        {
                            lastOnlineTimeUtc = sdcRecord.LastOnlineDateTimeUtc;
                            break;
                        }
                    }
                }

                record.LastOnlineDateTimeUtc = lastOnlineTimeUtc;
                record.OwnerStatus = DateTime.UtcNow.Subtract(record.LastOnlineDateTimeUtc).TotalSeconds < 60 ? DeviceOwnerStatus.Online : DateTime.UtcNow.Subtract(record.LastOnlineDateTimeUtc).TotalSeconds < 300 ? DeviceOwnerStatus.Away : DeviceOwnerStatus.Offline;
                records.Add(record);
            }
            else if (d.OwnerType == DeviceOwnerType.Guest)
            {
                if (devices.Any(x => x.ToLower() == d.DeviceName.ToLower()))                
                    records.Add(record);                
            }
            else
                records.Add(record);
        }
        var response = new TpLinkDecoResponse()
        {
            DateTimeUtc = DateTime.UtcNow,
            Records = records.ToArray()
        };

        return response;
    }
}
