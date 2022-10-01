using Cloudikka.PolylineAlgorithm;
using Microsoft.JSInterop;
using SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;
using SmartMirrorHubV6.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Common.Helpers;

namespace SmartMirrorHubV6.Website.Pages.Components.Sports
{
    public partial class StravaMapComponent : MirrorGenericBaseComponent<StravaMapResponse>
    {
        public override string ComponentAuthor => "Strava";
        public override string ComponentName => "Strava Map";
        public override bool IsOverlay() { return true; }
        private bool Initialized { get; set; }
        private string UniqueMapName { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            UniqueMapName = EncodingHelper.Base64Encode(Name).Replace("=", "");

            if (Response == null || IsShowing == false)
                return;

            if (firstRender)
                return;

            await JsRuntime.InvokeVoidAsync($"initMap{UniqueMapName}", null);

            var coordinates = PolylineAlgorithm.Decode(Response.EncodedPolyline.ToCharArray());
            if (coordinates?.Any() == true)
            {
                var parsedCoordinates = coordinates.Select(x => new Coordinate() { Latitude = x.Latitude, Longitude = x.Longitude });

                var jsonCoorindates = Newtonsoft.Json.JsonConvert.SerializeObject(parsedCoordinates);
                await JsRuntime.InvokeVoidAsync("updatePolylines", UniqueMapName, jsonCoorindates);
            }
        }

    }
}
