using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Hubs;
using SmartMirrorHubV6.Api.Models;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Utilities.Common.Helpers;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MirrorComponentController : BaseController
{
    private IHubContext<MirrorHub, IMirrorHub> MirrorHub;
    public MirrorComponentController(IUnitOfWork unitOfWork, IHubContext<MirrorHub, IMirrorHub> mirrorHub) : base(unitOfWork)
    {
        MirrorHub = mirrorHub;
    }

    [HttpGet("mirrorId/{mirrorId}", Name = "GetAllMirrorComponentsByMirrorId")]
    public async Task<MirrorComponent[]> GetAllByMirrorId(int mirrorId)
    {
        var mirrorComponents = await UnitOfWork.MirrorComponents.GetAllByMirrorId(mirrorId);
        return mirrorComponents.ToArray();
    }

    [HttpGet("userId/{userId}/mirrorName/{mirrorName}", Name = "GetAllMirrorComponentsByUserIdAndMirrorName")]
    public async Task<MirrorComponentResponse[]> GetAllByUserIdAndMirrorName(int userId, string mirrorName)
    {
        var responsesList = new List<MirrorComponentResponse>();
        var mirrorComponents = await UnitOfWork.MirrorComponents.GetAllByUserIdAndMirrorName(userId, mirrorName, false, true);
        if (mirrorComponents.Any() == false)
            return null;

        var mirror = await UnitOfWork.Mirrors.GetById(mirrorComponents[0].MirrorId);
        if (mirror == null)
            return null;

        foreach (var mc in mirrorComponents)
        {
            var component = await UnitOfWork.Components.GetById(mc.ComponentId);
            if (component == null)
                continue;

            var response = new MirrorComponentResponse()
            {
                MirrorId = mc.MirrorId,
                MirrorComponentId = mc.Id,
                Name = mc.Name,
                UiElement = mc.UiElement,
                ComponentName = component.Name,
                ComponentAuthor = component.Author,
                ComponentHasJavaScript = component.HasJavaScript,
                InSchedule = mc.ShowMirrorComponent(mc, mirror)
            };

            responsesList.Add(response);
        }

         return responsesList.ToArray();
    }

    [HttpGet("mirrorComponentId/{mirrorComponentId}", Name = "GetMirrorComponent")]
    public async Task<MirrorComponent> Get(int mirrorComponentId)
    {
        var mirrorComponent = await UnitOfWork.MirrorComponents.GetById(mirrorComponentId, true);
        return mirrorComponent;
    }

    [HttpGet("mirrorComponentId/{mirrorComponentId}/js", Name = "GetMirrorComponentJavaScript")]
    public async Task<byte[]> GetJavaScript(int mirrorComponentId)
    {
        var mirrorComponent = await Get(mirrorComponentId);
        if (mirrorComponent == null)
            return null;

        var dbComponent = await UnitOfWork.Components.GetById(mirrorComponent.ComponentId, true);
        if (dbComponent == null)
            return null;

        var settings = mirrorComponent.Settings.Select(x => (PropertyName: dbComponent.Settings.FirstOrDefault(z => z.Id == x.ComponentSettingId).Name, Value: GetValue(x, dbComponent.Settings.FirstOrDefault(z => z.Id == x.ComponentSettingId).Type)));
        var component = BaseComponent.GetComponent(dbComponent.Author, dbComponent.Name, settings.ToArray());
        if (component == null)
            return null;

        var js = component.GetJavaScript(mirrorComponent.Name);
        return Encoding.ASCII.GetBytes(js);
    }

    [HttpPost("mirrorComponentId/{mirrorComponentId}/toggle", Name = "ShowMirrorComponent")]
    public async Task ToggleMirrorComponent(int mirrorComponentId, [FromQuery] bool show)
    {
        var mirrorComponent = await UnitOfWork.MirrorComponents.GetById(mirrorComponentId, true, true);
        if (mirrorComponent == null)
            return;

        var mirror = await UnitOfWork.Mirrors.GetById(mirrorComponent.MirrorId);
        if (mirror == null)
            return;

        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").SwitchMirrorLayer(mirror.UserId, mirror.Name, mirrorComponent.UiElement.Layer);
        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").ToggleMirrorComponents(mirror.UserId, mirror.Name, show, new string[] { mirrorComponent.Name });
    }

    [HttpPost("voiceDeviceId/{voiceDeviceId}/toggle", Name = "ShowMirrorComponentByVoice")]
    public async Task ToggleMirrorComponentByVoice(string voiceDeviceId, [FromQuery] string toggleType, [FromQuery] string voiceName)
    {
        var mirror = await UnitOfWork.Mirrors.GetByVoiceDeviceId(voiceDeviceId, true);
        if (mirror == null)
            return;

        var components = await UnitOfWork.Components.GetAll();
        var component = components.FirstOrDefault(x => x.VoiceName.ToLower() == voiceName.ToLower());
        if (component == null)
            return;

        var mirrorComponent = mirror.MirrorComponents.FirstOrDefault(x => x.ComponentId == component.Id);
        if (mirrorComponent == null)
            return;

        mirrorComponent = await UnitOfWork.MirrorComponents.GetById(mirrorComponent.Id, true, true);

        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").SwitchMirrorLayer(mirror.UserId, mirror.Name, mirrorComponent.UiElement.Layer);
        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").ToggleMirrorComponents(mirror.UserId, mirror.Name, toggleType == "show", new string[] { mirrorComponent.Name });
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

            if (component.Type == ComponentType.OAuthApi)
            {
                if (mirrorComponent.TokenId != null)
                {
                    var token = await UnitOfWork.Tokens.GetById(mirrorComponent.TokenId.Value);
                    if (token != null)
                    {
                        var apiOAuthComponent = (ApiOAuthComponent)component;
                        await UpdateOAuthToken(apiOAuthComponent, token);
                    }
                }
            }

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

    [HttpGet("mirrorComponentId/{mirrorComponentId}/refresh", Name = "RefreshMirrorComponent")]
    public async Task Refresh(int mirrorComponentId)
    {
        var mirrorComponent = await UnitOfWork.MirrorComponents.GetById(mirrorComponentId, true, true);
        if (mirrorComponent == null)
            return;

        var mirror = await UnitOfWork.Mirrors.GetById(mirrorComponent.MirrorId);
        if (mirror == null)
            return;

        var component = await UnitOfWork.Components.GetById(mirrorComponent.ComponentId);
        if (component == null)
            return;

        mirrorComponent.InSchedule = mirrorComponent.ShowMirrorComponent(mirrorComponent, mirror);

        var response = await Retrieve(mirrorComponentId);
        var refreshResponse = new RefreshComponentResponse()
        {
            MirrorComponentId = mirrorComponent.Id,
            ComponentResponse = mirrorComponent.InSchedule ? response : new ComponentResponse() { Error = "Component is not in schedule" }
        };

        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").SwitchMirrorLayer(mirror.UserId, mirror.Name, mirrorComponent.UiElement.Layer);
        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").RefreshMirrorComponents(mirror.UserId, mirror.Name, new RefreshComponentResponse[] { refreshResponse });
    }


    [HttpGet("voiceDeviceId/{voiceDeviceId}/refresh", Name = "RefreshMirrorComponentByVoice")]
    public async Task RefreshByVoice(string voiceDeviceId, [FromQuery] string voiceName)
    {
        var mirror = await UnitOfWork.Mirrors.GetByVoiceDeviceId(voiceDeviceId, true);
        if (mirror == null)
            return;

        var components = await UnitOfWork.Components.GetAll();
        var component = components.FirstOrDefault(x => x.VoiceName.ToLower() == voiceName.ToLower());
        if (component == null)
            return;

        var mirrorComponent = mirror.MirrorComponents.FirstOrDefault(x => x.ComponentId == component.Id);
        if (mirror == null)
            return;

        mirrorComponent = await UnitOfWork.MirrorComponents.GetById(mirrorComponent.Id, true, true);
        mirrorComponent.InSchedule = mirrorComponent.ShowMirrorComponent(mirrorComponent, mirror);

        var response = await Retrieve(mirrorComponent.Id);
        var refreshResponse = new RefreshComponentResponse()
        {
            MirrorComponentId = mirrorComponent.Id,
            ComponentResponse = mirrorComponent.InSchedule ? response : new ComponentResponse() { Error = "Component is not in schedule" }
        };

        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").SwitchMirrorLayer(mirror.UserId, mirror.Name, mirrorComponent.UiElement.Layer);
        await MirrorHub.Clients.Groups($"{mirror.UserId}:{mirror.Name}").RefreshMirrorComponents(mirror.UserId, mirror.Name, new RefreshComponentResponse[] { refreshResponse });
    }


    [Produces(typeof(object))]
    [HttpGet("mirrorComponentId/{mirrorComponentId}/history", Name = "GetLatestHistoryMirrorComponent")]
    public async Task<object> GetHistory(int mirrorComponentId)
    {
        var history = await UnitOfWork.ResponseHistory.GetLatestByMirrorComponentId(mirrorComponentId);
        if (history == null)
            return new ComponentResponse() { Error = "No history has been recorded" };

        var response = history.Response;
        return JsonSerializer.Deserialize<object>(response);
    }

    private async Task UpdateOAuthToken(ApiOAuthComponent component, Token token)
    {
        component.RefreshToken = token.RefreshToken;
        component.AccessToken = token.AccessToken;

        DateTime expiry;
        if (token.ExpiresAt > 0)
            expiry = DateTimeOffset.FromUnixTimeSeconds(token.ExpiresAt).UtcDateTime;
        else
            expiry = DateTime.UtcNow.AddSeconds(token.ExpiresIn);

        if (expiry.CompareTo(DateTime.UtcNow) < 0 || component.RefreshBeforeExpired)
        {
            var restInstance = RestService.Instance.SetContentType(component.RefreshContentType);
            if (component.RefreshUseBearer)
            {
                restInstance = restInstance.SetAuthorizationHeader(("Basic", EncodingHelper.Base64Encode($"{component.ClientId}:{component.ClientSecret}")));
            }

            OAuthRefreshTokenResponse refreshResponse;
            if (component.RefreshTokenUrlMethod == "GET")
                refreshResponse = await restInstance.Get<OAuthRefreshTokenResponse>($"{component.RefreshTokenUrl}{component.RefreshTokenQueryString}");
            else
            {
                if (component.RefreshContentType == "x-www-form-urlencoded")
                {
                    var keyValuePair = new List<KeyValuePair<string, string>>
                            {
                                new KeyValuePair<string, string>("grant_type", component.RefreshGrantType),
                                new KeyValuePair<string, string>("refresh_token", token.RefreshToken)
                            };
                    var encodedContent = new FormUrlEncodedContent(keyValuePair);
                    refreshResponse = await restInstance.Post<OAuthRefreshTokenResponse>(component.RefreshTokenUrl, encodedContent);
                }
                else
                    refreshResponse = await restInstance.Post<OAuthRefreshTokenResponse>($"{component.RefreshTokenUrl}{component.RefreshTokenQueryString}");
            }

            if (refreshResponse != null)
            {
                token.AccessToken = refreshResponse.AccessToken;
                if (!string.IsNullOrEmpty(refreshResponse.RefreshToken))
                    token.RefreshToken = refreshResponse.RefreshToken;

                token.ExpiresIn = refreshResponse.ExpiresIn;

                if (refreshResponse.ExpiresAt != 0)
                    token.ExpiresAt = refreshResponse.ExpiresAt;
                else
                    token.ExpiresAt = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds + refreshResponse.ExpiresIn;

                await UnitOfWork.Tokens.Update(token);
            }
        }

        // set again as it might be updated
        component.AccessToken = token.AccessToken;
    }

    private object GetValue(MirrorComponentSetting setting, string typeName)
    {
        var type = Type.GetType(typeName);
        if (type == null)
            type = Type.GetType(typeName + ", SmartMirrorHubV6.Shared");

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

    public class OAuthRefreshTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string GrantType { get; set; }
        public string RedirectUri { get; set; }

        public OAuthRefreshTokenRequest()
        {
            RedirectUri = "https://localhost/";
        }
    }

    public class OAuthRefreshTokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_at")]
        public int ExpiresAt { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
    }
}
