using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Enums;
using System.Reflection;
using System.Text.Json;

namespace SmartMirrorHubV6.Shared.Components.Base;

public abstract class BaseComponent
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract string Author { get; }
    public abstract ComponentCategory Category { get; }
    public abstract ComponentType Type { get; }
    public abstract int Interval { get; }
    public abstract string VoiceName { get; }
    public virtual string GetJavaScript(string name) { return ""; }

    protected abstract Task<ComponentResponse> Get();
    public async Task<ComponentResponse> Retrieve()
    {
        return await Get();
    }

    public static BaseComponent GetComponent(string componentAuthor, string componentName, (string PropertyName, object Value)[] settings)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(BaseComponent)) && x.IsAbstract == false).ToArray();

        foreach (var c in types)
        {
            var instance = (BaseComponent)Activator.CreateInstance(c);
            if (instance.Author != componentAuthor || instance.Name != componentName)
                continue;

            var properties = c.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var p in properties.Where(x => Attribute.IsDefined(x, typeof(ComponentInputAttribute))))
            {
                var setting = settings.FirstOrDefault(x => x.PropertyName == p.Name);
                p.SetValue(instance, setting.Value);
            }

            return instance;
        }

        return null;
    }

}
