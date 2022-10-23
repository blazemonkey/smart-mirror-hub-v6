using SmartMirrorHubV6.Shared.Components.Data.Music;

namespace SmartMirrorHubV6.Website.Pages.Components.Music;

public partial class SpotifyComponent : MirrorGenericBaseComponent<SpotifyResponse>
{
    public override string ComponentAuthor => "Spotify";
    public override string ComponentName => "Currently Playing Track";
}
