namespace SmartMirrorHubV6.Shared.Attributes;

public class ComponentHistoryAttribute : Attribute
{
    public int MirrorComponentId { get; set; }
    public ComponentHistoryAttribute(int mirrorComponentId)
    {
        MirrorComponentId = mirrorComponentId;
    }
}
