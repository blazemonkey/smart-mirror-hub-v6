using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Enums;

namespace SmartMirrorHubV6.Shared.Components.Base;

public abstract class ApiComponent : ApiBaseComponent
{
    #region Inputs
    [ComponentInput("Authorization", "Access Token")]

    public string AccessToken { get; set; }
    #endregion

    public override ComponentType Type => ComponentType.Api;
}
