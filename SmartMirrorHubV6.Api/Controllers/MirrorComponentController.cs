using Microsoft.AspNetCore.Mvc;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Shared.Components.Base;
using System.Diagnostics;
using System.Text.Json;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MirrorComponentController : BaseController
{
    public MirrorComponentController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [HttpGet("mirrorId/{mirrorId}", Name = "GetAllMirrorComponentsByMirrorId")]
    public async Task<MirrorComponent[]> GetAllByMirrorId(int mirrorId)
    {
        var mirrorComponents = await UnitOfWork.MirrorComponents.GetAllByMirrorId(mirrorId);
        return mirrorComponents.ToArray();
    }

    [HttpGet("mirrorComponentId/{mirrorComponentId}", Name = "GetMirrorComponent")]
    public async Task<MirrorComponent> Get(int mirrorComponentId)
    {
        var mirrorComponent = await UnitOfWork.MirrorComponents.GetById(mirrorComponentId, true);
        return mirrorComponent;
    }

    [HttpGet("mirrorComponentId/{mirrorComponentId}/retrieve", Name = "RetrieveMirrorComponent")]
    public async Task<ComponentResponse> Retrieve(int mirrorComponentId)
    {
        var mirrorComponent = await Get(mirrorComponentId);
        if (mirrorComponent == null)
            return new ComponentResponse() { Error = "Could not find Mirror Component" };
        else if (mirrorComponent.Active == false)
            return new ComponentResponse() { Error = "Mirror Component is not Active" };

        var mirror = await UnitOfWork.Mirrors.GetById(mirrorComponent.MirrorId);
        if (mirror == null)
            return new ComponentResponse() { Error = "Could not find Mirror" };
        else if (mirror.Live == false)
            return new ComponentResponse() { Error = "Mirror is not Live" };

        var dbComponent = await UnitOfWork.Components.GetById(mirrorComponent.ComponentId, true);
        var settings = mirrorComponent.Settings.Select(x => (PropertyName: dbComponent.Settings.FirstOrDefault(z => z.Id == x.ComponentSettingId).Name, Value: GetValue(x, dbComponent.Settings.FirstOrDefault(z => z.Id == x.ComponentSettingId).Type)));

        var component = BaseComponent.GetComponent(dbComponent.Author, dbComponent.Name, settings.ToArray());
        if (component == null)
            return new ComponentResponse() { Error = "Could not find Component" };

        ComponentResponse response = null;
        var stopwatch = new Stopwatch();
        try
        {
            stopwatch.Start();
            response = await component.Retrieve();
        }
        catch (Exception ex)
        {
            response = new ComponentResponse() { Error = $"An error occured retrieving the Component: {ex.Message}" };
        }
        finally
        {
            stopwatch.Stop();
            var success = string.IsNullOrEmpty(response?.Error);
            new Task(() => UnitOfWork.ResponseHistory.Insert(new ResponseHistory()
            {
                DateTimeUtc = DateTime.UtcNow,
                MirrorComponentId = mirrorComponent.Id,
                Success = success,
                TimeTaken = stopwatch.Elapsed,
                Response = response == null ? "" : JsonSerializer.Serialize<object>(response)
            })).Start();

            if (success && component.Interval > 0)
            {
                mirrorComponent.LastUpdatedTimeUtc = DateTime.UtcNow;
                new Task(() => UnitOfWork.MirrorComponents.Update(mirrorComponent)).Start();                
            }
        }

        return response;
    }

    [HttpGet("mirrorComponentId/{mirrorComponentId}/history", Name = "GetLatestHistoryMirrorComponent")]
    public async Task<object> GetHistory(int mirrorComponentId)
    {
        var history = await UnitOfWork.ResponseHistory.GetLatestByMirrorComponentId(mirrorComponentId);
        if (history == null)
            return new ComponentResponse() { Error = "No history has been recorded" };

        var response = history.Response;
        return JsonSerializer.Deserialize<object>(response);
    }

    private object GetValue(MirrorComponentSetting setting, string typeName)
    {
        var type = Type.GetType(typeName); 
        if (type == typeof(int))
            return int.Parse(setting.IntValue.ToString());
        else if (type == typeof(string))
            return setting.StringValue;
        else if (type == typeof(bool))
            return setting.BoolValue;
        else if (type == typeof(DateTime))
            return setting.DateTimeValue;
        else
            return JsonSerializer.Deserialize(setting.JsonValue, type);
    }
}
