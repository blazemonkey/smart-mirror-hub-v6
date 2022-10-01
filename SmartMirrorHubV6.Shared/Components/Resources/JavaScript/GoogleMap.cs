using Utilities.Common.Helpers;

namespace SmartMirrorHubV6.Shared.Components.Resources.JavaScript;

public class GoogleMap
{
    public static string GetInit(string uniqueName, string mapKey, string mapStyle)
    {
        var base64Encoded = EncodingHelper.Base64Encode(uniqueName).Replace("=", "");
        var script = $@"var script{base64Encoded} = document.createElement('script');
                script{base64Encoded}.src = 'https://maps.googleapis.com/maps/api/js?key={mapKey}&callback=initMap'
                script{base64Encoded}.defer = true;
                window.initMap{base64Encoded} = function() {{
                    var latlng = new google.maps.LatLng(40.716948, -74.003563);
                    var options = {{
                        zoom: 14,
                        center: latlng,
                        disableDefaultUI: true,
                        mapTypeId: google.maps.MapTypeId.ROADMAP,
                        styles: {mapStyle}
                    }};
                    window['googleMap{base64Encoded}'] = new google.maps.Map(document.getElementById('map{base64Encoded}'), options);
                }};

                document.head.appendChild(script{base64Encoded});";

        return script;
    }
}
