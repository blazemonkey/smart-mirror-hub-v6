namespace SmartMirrorHubV6.Shared.Attributes;

public class ComponentDependsAttribute : Attribute
{
    public string MirrorComponentName { get; set; }
    public string MirrorComponentIdColumnName { get; set; }
    public ComponentDependsAttribute(string mirrorComponentName, string mirrorComponentIdColumnName)
    {
        MirrorComponentName = mirrorComponentName;
        MirrorComponentIdColumnName = mirrorComponentIdColumnName;
    }
}
