using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Components.Data.SocialMedia;
using SmartMirrorHubV6.Shared.Enums;
using System.Xml.Linq;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.CouncilCollection;

public class NZRubbishComponent : ApiComponent
{
    public override string Name => "New Zealand Rubbish Collection";
    public override string Description => "Get rubbish collection dates from New Zealand";
    public override string Author => "NZ Rubbish";
    public override string BaseUrl => Url;
    public override ComponentCategory Category => ComponentCategory.CouncilCollection;
    public override int Interval => 60 * 60 * 24;
    public override string VoiceName => "Rubbish";

    #region Inputs
    [ComponentInput("Url")]
    public string Url { get; set; }
    [ComponentInput("Council Type")]
    public int CouncilType { get; set; }
    [ComponentInput("Street Address")]
    public string StreetAddress { get; set; }
    #endregion

    protected override async Task<ComponentResponse> Get()
    {
        var url = Url.Replace("{council}", CouncilType.ToString()).Replace("{streetAddress}", StreetAddress);

        var collectionResponse = await RestService.Instance.Get<CollectionResponse>(url);
        var collections = collectionResponse.Details.Select(x => new RubbishCollectionDetailResponse() {  CollectionDate = x.Date, Type = x.Type });

        var response = new RubbishCollectionResponse
        {
            StreetAddress = collectionResponse.StreetAddress,
            Collections = collections.ToArray()
        };

        return response;
    }
}
