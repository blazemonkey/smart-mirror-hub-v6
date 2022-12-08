using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartMirrorHubV6.Shared.Components.Base;

public abstract class DockerComponent : BaseComponent
{
    public override ComponentType Type => ComponentType.Docker;

    [ComponentInput("Output Directory")]
    public string OutputDirectory { get; set; }
    [ComponentInput("Output Filename")]
    public string OutputFilename { get; set; }
}
