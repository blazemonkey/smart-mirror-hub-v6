using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartMirrorHubV6.Website.Pages
{
    public partial class Index : BaseComponent
    {
        protected override void OnInitialized()        
        {
            base.OnInitialized();
#if DEBUG
            NavigationManager.NavigateTo("/mirror/1/Dev");
#else
            NavigationManager.NavigateTo($"/mirror/{Settings.UserId}/{Settings.MirrorName}");
#endif
        }
    }
}
