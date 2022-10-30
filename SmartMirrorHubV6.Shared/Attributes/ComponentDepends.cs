namespace SmartMirrorHubV6.Shared.Attributes;

public class ComponentDependsAttribute : Attribute
{
    public string Name { get; set; }
    public bool GetLatest { get; set; }
    public ComponentDependsAttribute(string name, bool getLatest)
    {
        Name = name;
        GetLatest = getLatest;
    }
}
