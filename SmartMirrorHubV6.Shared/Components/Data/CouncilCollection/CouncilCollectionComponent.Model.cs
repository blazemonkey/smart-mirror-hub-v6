using SmartMirrorHubV6.Shared.Components.Base;
using System.ComponentModel;

namespace SmartMirrorHubV6.Shared.Components.Data.CouncilCollection;

public class CouncilCollectionResponse : ComponentResponse
{
    public string StreetAddress { get; set; }
    public CouncilCollectionDetailResponse[] Collections { get; set; }

}

public class CouncilCollectionDetailResponse
{
    public CollectionType Type { get; set; }
    public DateTime CollectionDate { get; set; }
}

public enum CollectionType
{
    [Description("Rubbish")]
    Rubbish = 0,
    [Description("Recycling;Recycle")]
    Recycling = 1
}
