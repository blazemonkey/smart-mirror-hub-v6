using Microsoft.JSInterop;
using PolylinerNet;
using SmartMirrorHubV6.Shared.Components.Data.Maps;
using SmartMirrorHubV6.Website.Models;
using System.Text.Json;
using Utilities.Common.Helpers;

namespace SmartMirrorHubV6.Website.Pages.Components.Maps;

public partial class GoogleMapsComponent : MirrorGenericBaseComponent<GoogleMapsResponse>
{
    public override string ComponentAuthor => "Google";
    public override string ComponentName => "Map Directions";
    private bool Initialized { get; set; }
    private string UniqueMapName { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        UniqueMapName = EncodingHelper.Base64Encode(Name).Replace("=", "");
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Response == null || Response.ShowMap == false || IsShowing == false || firstRender)
        {
            Initialized = false;
            return;
        }

        if (Initialized == false)
        {
            await Task.Delay(2000); // we put a delay here so that we ensure the mirror-scripts.js has finished loaded regardless of cache
            await JsRuntime.InvokeVoidAsync($"initMap{UniqueMapName}", null);
            Initialized = true;
        }

        var fastestRoute = Response.Routes?.OrderBy(x => x.DurationInSeconds)?.FirstOrDefault();
        if (fastestRoute == null)
            return;

        var polyliner = new Polyliner();
        var coordinates = polyliner.Decode(fastestRoute.Polyline);
        if (coordinates?.Any() == true)
        {
            var parsedCoordinates = coordinates.Select(x => new Coordinate() { Latitude = x.Latitude, Longitude = x.Longitude });

            var jsonCoorindates = JsonSerializer.Serialize(parsedCoordinates);
            await JsRuntime.InvokeVoidAsync("updatePolylines", UniqueMapName, jsonCoorindates);
        }
    }

}
