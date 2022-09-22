using HtmlAgilityPack;
using SmartMirrorHubV6.Shared.Enums;

namespace SmartMirrorHubV6.Shared.Components.Base;

public abstract class ScrapeComponent : BaseComponent
{
    public abstract string Url { get; }
    public override ComponentType Type => ComponentType.Scrape;

    protected HtmlDocument GetDocument(string url)
    {
        var web = new HtmlWeb();
        var doc = web.Load(url);

        return doc;
    }
}
