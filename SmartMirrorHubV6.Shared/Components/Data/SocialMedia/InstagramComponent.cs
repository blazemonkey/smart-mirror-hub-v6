using SmartMirrorHubV6.Shared.Attributes;
using SmartMirrorHubV6.Shared.Components.Base;
using SmartMirrorHubV6.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Common.RestService;

namespace SmartMirrorHubV6.Shared.Components.Data.SocialMedia;

public class InstagramComponent : ApiOAuthComponent
{
    public override string Name => "Instagram Media";
    public override string Description => "Get latest media from Instagram";
    public override string Author => "Instagram";
    public override string BaseUrl => "https://graph.instagram.com/";
    public override ComponentCategory Category => ComponentCategory.SocialMedia;
    public override int Interval => 60 * 60 * 24;
    public override string VoiceName => "Instagram";

    #region OAuth
    public override string AuthorizeUrl => "https://api.instagram.com/oauth/authorize";
    public override string RefreshTokenUrl => "https://graph.instagram.com/refresh_access_token";
    public override string RefreshGrantType => "ig_refresh_token";
    public override string RefreshTokenUrlMethod => "GET";
    public override string RefreshTokenQueryString => $"?grant_type={RefreshGrantType}&access_token={AccessToken}";
    public override bool RefreshBeforeExpired => true;
    #endregion

    #region Inputs
    [ComponentInput("Max Number of Images")]
    public int MaxNumberOfImages { get; set; }
    [ComponentInput("Max Image Height")]
    public int MaxImageHeight { get; set; }
    [ComponentInput("Max Image Width")]
    public int MaxImageWidth { get; set; }
    [ComponentInput("Seconds Between Images")]
    public int SecondsBetweenImages { get; set; }
    [ComponentInput("Username")]
    public string Username { get; set; }
    #endregion

    public override async Task<ComponentResponse> GetOAuthApi()
    {
        var media = new List<InstagramData>();

        await QueryMediaList(media, $"{BaseUrl}me/media?fields=id,caption,media_type,media_url&access_token={AccessToken}");
        if (!media.Any())
            return new ComponentResponse() { Error = "No media were found" };

        var imageUrls = new List<string>();
        foreach (var m in media)
        {
            if (m.MediaType == "IMAGE")
                imageUrls.Add(m.MediaUrl);
            else if (m.MediaType == "CAROUSEL_ALBUM")
            {
                var childrenMedia = new List<InstagramData>();
                await QueryMediaList(childrenMedia, $"{BaseUrl}{m.Id}/children?fields=id,media_type,media_url&access_token={AccessToken}");

                foreach (var c in childrenMedia)
                {
                    if (c.MediaType == "IMAGE")
                        imageUrls.Add(c.MediaUrl);
                }
            }
        }

        var response = new InstagramMediaListResponse()
        {
            Username = Username,
            MaxImageHeight = MaxImageHeight,
            MaxImageWidth = MaxImageWidth,
            SecondsBetweenImages = SecondsBetweenImages,
            MediaUrls = imageUrls.Take(MaxNumberOfImages).ToArray()
        };

        return response;
    }

    private async Task QueryMediaList(List<InstagramData> media, string url)
    {
        var list = await RestService.Instance.Get<InstagramMediaListRoot>(url);
        if (list == null)
            return;

        media.AddRange(list.Data);

        if (!string.IsNullOrEmpty(list.Paging?.Next) && media.Count() < MaxNumberOfImages)
            await QueryMediaList(media, list.Paging.Next);
    }
}

