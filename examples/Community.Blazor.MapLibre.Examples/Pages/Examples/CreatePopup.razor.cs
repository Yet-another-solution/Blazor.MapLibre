using Community.Blazor.MapLibre.Models;
using Community.Blazor.MapLibre.Models.Event;

namespace Community.Blazor.MapLibre.Examples.WebAssembly.Pages.Examples;

public partial class CreatePopup
{
    private readonly MapOptions _mapOptions = new()
    {
        Style = "https://demotiles.maplibre.org/style.json"
    };

    private async Task OnMapLoad(EventArgs args)
    {
        if (_map is null)
        {
            return;
        }

        await _map.OnClick(null, e => AddPopup(e));

        async void AddPopup(MapMouseEvent evnt)
        {
            await _map.CreatePopup(new Popup
            {
                Content = $"""
                           <div>
                               <h3>Map clicked</h3>
                               <p>You clicked the map at <code>{evnt.LngLat.Latitude} {evnt.LngLat.Longitude}</code></p>
                           </div>
                           """,
                Coordinates = evnt.LngLat
            }, new PopupOptions
            {
                CloseOnClick = true,
                CloseOnMove = true,
                FocusAfterOpen = true,
            });
        }
    }
}