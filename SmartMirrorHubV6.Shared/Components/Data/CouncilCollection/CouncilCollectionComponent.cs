using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirrorHubV6.Shared.Components.Data.CouncilCollection;

public abstract class CouncilCollectionComponent : ScrapeComponent
{
    public abstract string GetIdUrl { get; }
    [ComponentInput("Street Address")]
    public string StreetAddress { get; set; }
    protected string StreetId { get; set; }
    public override ComponentCategory Category => ComponentCategory.CouncilCollection;

    protected abstract Task<string> GetStreetId();
    protected abstract CouncilCollectionDetailResponse[] GetCollectionDates();
    protected abstract string GetStreetName();

    protected T GetEnumTypeByName<T>(string name)
    {
        var values = Enum.GetValues(typeof(T)).Cast<T>();
        foreach (var v in values)
        {
            var description = v.GetType().GetMember(v.ToString()).FirstOrDefault()?.GetCustomAttribute<DescriptionAttribute>();
            if (description != null)
            {
                if (description.Description.Split(';').Select(x => x.ToLower()).Contains(name))
                    return v;
            }
            else
            {
                if (v.ToString().ToLower() == name.ToLower())
                    return v;
            }
        }

        return default;
    }
}
