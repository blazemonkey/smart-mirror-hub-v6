using Microsoft.JSInterop;
using PolylinerNet;
using SmartMirrorHubV6.Shared.Components.Data.Sports.Strava;
using SmartMirrorHubV6.Website.Models;
using System.Text.Json;
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

        public StravaActivity CurrentActivity { get; set; }
        private int active;
        private Timer timer;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            UniqueMapName = EncodingHelper.Base64Encode(Name).Replace("=", "");

            if (Response == null || IsShowing == false || firstRender)
            {
                Initialized = false;
                if (timer != null)
                {
                    await timer.DisposeAsync();
                    timer = null;
                }

                return;
            }

            if (Initialized == false)
            {
                await Task.Delay(2000); // we put a delay here so that we ensure the mirror-scripts.js has finished loaded regardless of cache
                await JsRuntime.InvokeVoidAsync($"initMap{UniqueMapName}", null);

                if (timer == null)
                    timer = new Timer(TimerCallback, null, 60 * 1000, Timeout.Infinite);

                TimerCallback(null);
                Initialized = true;
            }
        }

        private async void TimerCallback(object state)
        {
            await InvokeAsync(async () =>
            {
                active++;
                if (active == Response.Activities.Length)
                    active = 0;

                CurrentActivity = Response.Activities[active];
                var polyliner = new Polyliner();
                var coordinates = polyliner.Decode(CurrentActivity.EncodedPolyline);
                if (coordinates?.Any() == true)
                {
                    var parsedCoordinates = coordinates.Select(x => new Coordinate() { Latitude = x.Latitude, Longitude = x.Longitude });

                    var jsonCoorindates = JsonSerializer.Serialize(parsedCoordinates);
                    await JsRuntime.InvokeVoidAsync("updatePolylines", UniqueMapName, jsonCoorindates);
                }

                StateHasChanged();
            });

            timer.Change(60 * 1000, Timeout.Infinite);
        }

    }
}
