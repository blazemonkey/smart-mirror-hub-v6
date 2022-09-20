namespace SmartMirrorHubV6.Shared.Attributes;

public class ComponentInputAttribute : Attribute
{
    public string Category { get; set; }
    public string Name { get; set; }

    public ComponentInputAttribute(string category, string name)
    {
        Category = category;
        Name = name;
    }

    public ComponentInputAttribute(string name)
    {
        Category = "Component";
        Name = name;
    }
}
