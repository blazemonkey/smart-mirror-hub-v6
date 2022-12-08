namespace SmartMirrorHubV6.Shared.Enums;

public enum ComponentType
{
    None = 0,
    /// <summary>
    /// API Components that use a simple Access Token
    /// </summary>
    Api = 1,
    /// <summary>
    /// Scrape Components that retrieve via HTML Scraping
    /// </summary>
    Scrape = 2,
    /// <summary>
    /// API Components that require OAuth Authentication
    /// </summary>
    OAuthApi = 3,
    /// <summary>
    /// Docker Components that retrieve via output from Dockers
    /// </summary>
    Docker = 4
}
