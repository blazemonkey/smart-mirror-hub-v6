using SmartMirrorHubV6.Shared.Components.Base;

namespace SmartMirrorHubV6.Shared.Components.Data.News;

public class NewsSource
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class NewsArticle
{
    public NewsSource Source { get; set; }
    public string Author { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string UrlToImage { get; set; }
    //public DateTime PublishedAt { get; set; }
    public string Content { get; set; }
}

public class NewsApiRoot
{
    public string Status { get; set; }
    public int TotalResults { get; set; }
    public NewsArticle[] Articles { get; set; }
}

public class NewsApiResponse : ComponentResponse
{
    public string[] Headlines { get; set; }
    public string CombinedHeadline { get; set; }

    public static explicit operator NewsApiResponse(NewsApiRoot news)
    {
        var response = new NewsApiResponse();
        var headlines = new List<string>();
        foreach (var r in news.Articles)
            headlines.Add(r.Title);

        response.Headlines = headlines.ToArray();
        response.CombinedHeadline = string.Join(" | ", response.Headlines.ToArray());
        return response;
    }
}
