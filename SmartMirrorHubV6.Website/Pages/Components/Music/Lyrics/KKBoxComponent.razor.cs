using SmartMirrorHubV6.Shared.Components.Data.Music.Lyrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMirrorHubV6.Website.Pages.Components.Music.Lyrics
{
    public partial class KKBoxComponent : MirrorGenericBaseComponent<LyricsResponse>
    {
        public override string ComponentAuthor => "KKBox";
        public override string ComponentName => "Lyrics";
        public override bool IsOverlay() { return true; }
    }
}
