using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Hubs;
using SmartMirrorHubV6.Api.Models;
using SmartMirrorHubV6.Shared.Components.Base;
using System.Text.Json;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MirrorController : BaseController
{
    private IHubContext<MirrorHub, IMirrorHub> MirrorHub;
    public MirrorController(IUnitOfWork unitOfWork, IHubContext<MirrorHub, IMirrorHub> mirrorHub) : base(unitOfWork)
    {
        MirrorHub = mirrorHub;
    }

    [HttpGet(Name = "GetAlMirrors")]
    public async Task<Mirror[]> GetAll([FromQuery] bool includeComponents, [FromQuery] bool checkSchedule)
    {
        var mirrors = await UnitOfWork.Mirrors.GetAll(includeComponents);
        if (checkSchedule)
        {
            foreach (var mirror in mirrors)
            {
                if (mirror.MirrorComponents == null)
                    continue;

                foreach (var mc in mirror.MirrorComponents)
                {
                    mc.InSchedule = mc.ShowMirrorComponent(mc, mirror);
                }
            }
        }

        return mirrors.ToArray();
    }

    [Produces(typeof(RefreshComponentResponse))]
    [HttpPost("{mirrorId}", Name = "RefreshMirrorComponentsByMirrorId")]
    public async Task RefreshMirrorComponentsByMirrorId(int mirrorId)
    {
        var mirror = await UnitOfWork.Mirrors.GetById(mirrorId, true);
        if (mirror == null)
            return;

        var responses = new List<RefreshComponentResponse>();
        foreach (var mc in mirror.MirrorComponents)
        {
            var component = await UnitOfWork.Components.GetById(mc.ComponentId);
            if (component == null)
                continue;

            if (component.Interval == 0)
                continue;

            var history = await UnitOfWork.ResponseHistory.GetLatestByMirrorComponentId(mc.Id);
            if (history == null)
                continue;

            mc.InSchedule = mc.ShowMirrorComponent(mc, mirror);
            var jsonResponse = history.Response;

            var refreshResponse = new RefreshComponentResponse()
            {
                MirrorComponentId = mc.Id,
                ComponentResponse = mc.InSchedule ? JsonSerializer.Deserialize<object>(jsonResponse) : new ComponentResponse() { Error = "Component is not in schedule" }
            };

            responses.Add(refreshResponse);
        }

        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").RefreshMirrorComponents(mirror.UserId, mirror.Name, responses.ToArray());
    }

    [HttpPost("{mirrorId}/switch", Name = "SwitchMirrorLayer")]
    public async Task SwitchMirrorLayer(int mirrorId, int layer)
    {
        var mirror = await UnitOfWork.Mirrors.GetById(mirrorId, true);
        if (mirror == null)
            return;

        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").SwitchMirrorLayer(mirror.UserId, mirror.Name, layer);
    }
}
