using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Web;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.Music.Lyrics;

public class KKBoxComponent : LyricsComponent // This component now uses a combination of API and Scraping, making a base class annoying
{
    public override string Name => "Lyrics";
    public override string Description => "Get lyrics for songs";
    public override string Author => "KKBox";
    //public override string Url => $"https://www.kkbox.com/hk/tc/search.php?word={ Uri.EscapeDataString(ArtistNames + " " + SongName) }&search=song";
    public override string BaseUrl => "https://www.kkbox.com/api/search/song";
    //public override string LyricsPageBaseUrl => $"https://www.kkbox.com";
    public override string VoiceName => "Lyrics";

    public override async Task<LyricsResponse> GetLyrics()
    {
        //var searchResults = new List<LyricsSearchResult>();
        //SearchPages(1, searchResults);

        var results = await RestService.Instance.Get<KKBoxRoot>($"{BaseUrl}?q={Uri.EscapeDataString(ArtistNames + " " + SongName)}&p=1&terr=hk&lang=tc");

        if (results.Status != "OK" || results.Data.Result.Any() == false)
            return new LyricsResponse() { Error = "No results could be found" };

        var matchUrlLink = "";
        foreach (var sr in results.Data.Result.Where(x => x.HasLyrics))
        {
            if ((string.Join(" ", sr.ArtistRoles.Select(x => x.Name.ToLower())) == ArtistNames.ToLower())
                && (sr.Name.ToLower() == SongName.ToLower()))
            {
                matchUrlLink = sr.Url;
                break;
            }
            else if ((string.Join(" ", sr.ArtistRoles.Select(x => x.Name.ToLower())) == ArtistNames.ToLower())
                && (sr.Name.ToLower().Contains(SongName.ToLower())))
            {
                matchUrlLink = sr.Url;
                break;
            }
            else if ((string.Join(" ", sr.ArtistRoles.Select(x => x.Name.ToLower())).Contains(ArtistNames.ToLower()))
                && (SongName.ToLower() == (sr.Name.ToLower())))
            {
                matchUrlLink = sr.Url;
                break;
            }
        }

        if (string.IsNullOrEmpty(matchUrlLink))
            matchUrlLink = results.Data.Result.FirstOrDefault(x => x.HasLyrics).Url;

        var web = new HtmlWeb();
        var doc = web.Load(matchUrlLink);

        var lyrics = doc.DocumentNode.SelectNodes("//p|//div").Where(x => x.HasClass("lyrics")).FirstOrDefault();
        if (lyrics == null)
            return new LyricsResponse() { Error = "Found song but no lyrics" };

        var fullLyrics = "";
        var columnCount = 1;
        var lineCount = 1;
        var pagedLyrics = new List<string>();
        foreach (var l in lyrics.ChildNodes)
        {
            var innerText = l.InnerText.Trim();
            var splitLines = innerText.Split(new string[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in splitLines)
            {
                if (string.IsNullOrEmpty(line))
                    continue;

                if (Regex.IsMatch(line, "(作詞|作曲|編曲|監製|監|監製)\\s*(:|：)"))
                    continue;

                fullLyrics += line + Environment.NewLine;
                if (line != Environment.NewLine || fullLyrics.EndsWith(Environment.NewLine + Environment.NewLine))
                    lineCount++;

                if (lineCount == MaxLinesPerColumn * columnCount && columnCount < MaxColumnShowCount)
                {
                    fullLyrics = HttpUtility.HtmlDecode(fullLyrics).Trim();
                    pagedLyrics.Add(fullLyrics);

                    fullLyrics = "";
                    columnCount++;
                }
            }
        }

        fullLyrics = HttpUtility.HtmlDecode(fullLyrics).Trim();
        pagedLyrics.Add(fullLyrics);

        var response = new LyricsResponse()
        {
            LyricsPages = pagedLyrics.ToArray()
        };

        await Task.CompletedTask;
        return response;
    }

    //private void SearchPages(int pageNumber, List<LyricsSearchResult> searchResults)
    //{
    //    var doc = GetDocument(Url + "&cur_page=" + pageNumber);

    //    var items = doc.DocumentNode.SelectNodes("//song-li");
    //    foreach (var i in items)
    //    {
    //        var hasLyrics = i.ParentNode.ParentNode.ChildNodes.Where(x => x.HasClass("song-icon") && x.ChildNodes.Any(z => z.HasClass("icon-lyrics")))?.FirstOrDefault();
    //        if (hasLyrics == null)
    //            continue;

    //        var songName = i.GetAttributeValue("title", string.Empty);
    //        var urlLink = i.GetAttributeValue("href", string.Empty);
    //        var artistName = i.ParentNode.ChildNodes.FirstOrDefault(x => x.HasClass("song-artist-album"))?.ChildNodes?.FirstOrDefault(x => x.Name == "a")?.GetAttributeValue("title", string.Empty);

    //        var searchResult = new LyricsSearchResult()
    //        {
    //            ArtistName = HttpUtility.HtmlDecode(artistName),
    //            SongName = HttpUtility.HtmlDecode(songName),
    //            UrlLink = LyricsPageBaseUrl + urlLink
    //        };

    //        searchResults.Add(searchResult);
    //    }

    //    if (!searchResults.Any() && pageNumber <= 10)
    //        SearchPages(++pageNumber, searchResults);
    //}

}
