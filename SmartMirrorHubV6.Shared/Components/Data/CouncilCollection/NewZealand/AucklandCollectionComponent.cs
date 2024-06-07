using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.CouncilCollection.NewZealand;

public class AucklandCollectionComponent : CouncilCollectionComponent
{
    public override string Name => "Rubbish Collection (Auckland, New Zealand)";
    public override string Description => "Get rubbish and recycling dates for Auckland, New Zealand";
    public override string Author => "Auckland City Council";
    public override string GetIdUrl => "https://www.aucklandcouncil.govt.nz/_vti_bin/ACWeb/ACservices.svc/GetMatchingPropertyAddresses";
    public override string Url => $"https://www.aucklandcouncil.govt.nz/rubbish-recycling/rubbish-recycling-collections/Pages/collection-day-detail.aspx?an={StreetId}";
    public override ComponentCategory Category => ComponentCategory.CouncilCollection;
    public override int Interval => 60 * 60 * 24;
    public override string VoiceName => "Rubbish";

    protected override async Task<ComponentResponse> Get()
    {
        var streetId = await GetStreetId();
        if (string.IsNullOrEmpty(streetId))
            return new ComponentResponse() { Error = "Could not find street address. Please enter full name including street number" };

        StreetId = streetId;
        var dates = GetCollectionDates();
        var name = GetStreetName();

        var response = new CouncilCollectionResponse
        {
            StreetAddress = name,
            Collections = dates
        };

        return response;
    }

    protected override async Task<string> GetStreetId()
    {
        var data = new AucklandCollectionRoot() { SearchText = StreetAddress };
        var result = await RestService.Instance.Post<List<AucklandCollectionKeyResponse>>(GetIdUrl, data);
        if (result == null || !result.Any())
            return string.Empty;

        var streetId = result.FirstOrDefault()?.ACRateAccountKey;
        return streetId;
    }

    protected override CouncilCollectionDetailResponse[] GetCollectionDates()
    {
        var dates = new List<CouncilCollectionDetailResponse>();
        var doc = GetDocument(Url);

        var nodes = doc.DocumentNode.Descendants("span").Where(x => x.HasClass("m-r-1") &&
                        x.ParentNode.ParentNode.Id.Contains("HouseholdBlock")).Select(x => x.ParentNode).ToList();

        foreach (var n in nodes)
        {
            var childNodes = n.ChildNodes.Where(x => x.GetClasses().Any(z => z.StartsWith("icon")));
            foreach (var cn in childNodes)
            {
                var icon = cn.GetClasses().FirstOrDefault(x => x.StartsWith("icon"));
                if (icon == null)
                    continue;

                var regex = Regex.Match(icon, "^icon-(?<type>\\w*)$");
                var match = regex.Groups["type"];
                if (!match.Success)
                    continue;

                var type = GetEnumTypeByName<CouncilCollectionType>(match.Value);
                var date = cn.ParentNode.FirstChild.InnerText;
                if (string.IsNullOrEmpty(date))
                    continue;

                var dateTime = ParseCollectionDate(date, DateTime.Now.Year);
                var detail = new CouncilCollectionDetailResponse() { Type = type, CollectionDate = dateTime };
                dates.Add(detail);
            }
        }

        return dates.ToArray();
    }

    protected override string GetStreetName()
    {
        var doc = GetDocument(Url);
        var streetName = doc.DocumentNode.SelectNodes("//h2").Where(x => x.HasClass("m-b-2")).FirstOrDefault()?.InnerText;

        return streetName;
    }

    private DateTime ParseCollectionDate(string date, int year)
    {
        var fullDate = $"{ date } { year }";
        var success = DateTime.TryParseExact(fullDate, "dddd d MMMM yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parseDate);
        if (!success)
            return ParseCollectionDate(date, year + 1);

        return parseDate;
    }
}