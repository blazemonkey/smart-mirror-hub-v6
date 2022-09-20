using SmartMirrorHubV6.Shared.Enums;

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
}
