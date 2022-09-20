using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System.Reflection;

namespace SmartMirrorHubV6.Api.Database.Models;

public class Component : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public ComponentCategory Category { get; set; }
    public ComponentType Type { get; set; }
    public int Interval { get; set; }
    public string VoiceName { get; set; }
    public bool HasJavaScript { get; set; }
    public ComponentSetting[] Settings { get; set; }

    public static explicit operator Component(BaseComponent component)
    {
        var dbComponent = new Component()
        {
            Author = component.Author,
            Category = component.Category,
            Description = component.Description,
            Name = component.Name,
            Type = component.Type,
            Interval = component.Interval,
            VoiceName = component.VoiceName,
        };

        return dbComponent;
    }

    public static Component[] GetComponents()
    {
        var types = typeof(BaseComponent).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(BaseComponent)) && x.IsAbstract == false).ToArray();

        var components = new List<Component>();
        foreach (var c in types)
        {
            var hasJavaScript = c.GetMethod(nameof(BaseComponent.GetJavaScript)).DeclaringType != typeof(BaseComponent);

            var instance = (BaseComponent)Activator.CreateInstance(c);
            var component = (Component)instance;

            var settings = new List<ComponentSetting>();
            var properties = instance.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in properties)
            {
                var componentInputAttribute = p.GetCustomAttribute<ComponentInputAttribute>();
                if (componentInputAttribute == null)
                    continue;

                var setting = new ComponentSetting()
                {
                    ComponentId = component.Id,
                    Name = p.Name,
                    DisplayName = componentInputAttribute.Name,
                    Type = p.PropertyType.ToString()
                };

                settings.Add(setting);
            }

            component.HasJavaScript = hasJavaScript;
            component.Settings = settings.ToArray();
            components.Add(component);
        }

        return components.ToArray();
    }
}
