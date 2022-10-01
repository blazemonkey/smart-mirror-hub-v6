using SmartMirrorHubV6.Shared.Components.Data.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMirrorHubV6.Website.Pages.Components.Calendar
{
    public partial class LunarCalendarComponent : MirrorGenericBaseComponent<LunarCalendarResponse>
    {
        public override string ComponentAuthor => "Nong Li";
        public override string ComponentName => "Lunar Calendar";
    }
}
