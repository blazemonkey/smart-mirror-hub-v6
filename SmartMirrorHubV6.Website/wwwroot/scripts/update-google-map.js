function initMap() { }
function updatePolylines(uniqueName, coords) {
    var coordinates = JSON.parse(coords);

    var googleCoordinates = new Array();
    for (i = 0; i < coordinates.length; i++) {
        var point = new google.maps.LatLng(coordinates[i].Latitude, coordinates[i].Longitude);
        googleCoordinates.push(point);
    }

    if (window['path'.concat(uniqueName)] != undefined)
        window['path'.concat(uniqueName)].setMap(null);

    window['path'.concat(uniqueName)] = new google.maps.Polyline({
        path: googleCoordinates,
        geodesic: true,
        strokeColor: "#FFFFFF",
        strokeOpacity: 1.0,
        strokeWeight: 4,
    });

    var bounds = new google.maps.LatLngBounds();
    for (var i = 0; i < googleCoordinates.length; i++) {
        bounds.extend(googleCoordinates[i]);
    }

    var googleMapInstance = window['googleMap'.concat(uniqueName)];
    window['path'.concat(uniqueName)].setMap(googleMapInstance);
    googleMapInstance.fitBounds(bounds);
}