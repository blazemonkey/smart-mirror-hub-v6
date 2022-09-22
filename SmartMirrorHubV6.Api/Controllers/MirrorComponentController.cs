using Microsoft.AspNetCore.Mvc;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Utilities.Common.Helpers;
using Utilities.Common.RestService;

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

    private async Task UpdateOAuthToken(ApiOAuthComponent component, Token token)
    {
        component.RefreshToken = token.RefreshToken;
        
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

        component.AccessToken = token.AccessToken;
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
