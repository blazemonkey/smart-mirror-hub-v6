using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Enums;

namespace SmartMirrorHubV6.Shared.Components.Base;

public abstract class ApiOAuthComponent : ApiBaseComponent
{
    public override ComponentType Type => ComponentType.OAuthApi;

    public virtual string RefreshGrantType { get { return "refresh_token"; } }
    public abstract string AuthorizeUrl { get; }
    public virtual string RefreshTokenUrlMethod { get { return "POST"; } }
    public abstract string RefreshTokenUrl { get; }
    public abstract string RefreshTokenQueryString { get; }
    public virtual bool RefreshBeforeExpired { get { return false; } }
    public virtual bool RefreshUseBearer { get { return false; } }
    public virtual string RefreshContentType { get { return "json"; } }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    #region Inputs
    [ComponentInput("OAuth Client Id")]
    public string ClientId { get; set; }
    [ComponentInput("OAuth Client Secret")]
    public string ClientSecret { get; set; }
    [ComponentInput("OAuth Redirect Uri")]
    public string RedirectUri { get; set; }
    #endregion

    public abstract Task<ComponentResponse> GetOAuthApi();

    protected override sealed async Task<ComponentResponse> Get()
    {
        var result = await GetOAuthApi();
        return result;
    }
}
