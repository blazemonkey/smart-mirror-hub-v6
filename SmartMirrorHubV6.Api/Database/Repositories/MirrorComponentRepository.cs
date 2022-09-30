using Dapper;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class MirrorComponentRepository : BaseRepository, IMirrorComponentRepository
{
    public MirrorComponentRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<MirrorComponent[]> GetAllByMirrorId(int mirrorId)
    {
        var sql = "select * from mirrors_components where mirrorid = @MirrorId";
        using var conn = await OpenConnection();
        var result = await conn.QueryAsync<MirrorComponent>(sql, new { MirrorId = mirrorId });
        return result.ToArray();
    }

    public async Task<MirrorComponent> GetById(int id, bool includeSettings = false)
    {
        var sql = "select * from mirrors_components where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<MirrorComponent>(sql, new { Id = id });
        if (result == null)
            return null;

        if (includeSettings)
        {
            sql = "select * from mirrors_components_settings where mirrorcomponentid = @MirrorComponentId";
            var settings = await conn.QueryAsync<MirrorComponentSetting>(sql, new { MirrorComponentId = id });
            result.Settings = settings.ToArray();
        }

        return result;
    }

    public async Task<MirrorComponent[]> GetAllByUserIdAndMirrorName(int userId, string mirrorName, bool includeSettings = false, bool includeUiElement = false)
    {
        var sql = "select mc.* from mirrors m join mirrors_components mc on m.id = mc.mirrorid where userId = @userId and m.name = @MirrorName";
        using var conn = await OpenConnection();
        var result = await conn.QueryAsync<MirrorComponent>(sql, new { UserId = userId, MirrorName = mirrorName });
        if (result == null)
            return null;

        if (includeSettings)
        {
            foreach (var r in result)
            {
                sql = "select * from mirrors_components_settings where mirrorcomponentid = @MirrorComponentId";
                var settings = await conn.QueryAsync<MirrorComponentSetting>(sql, new { MirrorComponentId = r.Id });
                r.Settings = settings.ToArray();
            }
        }

        if (includeUiElement)
        {
            foreach (var r in result)
            {
                sql = "select * from mirrors_components_ui_elements where mirrorcomponentid = @MirrorComponentId";
                var element = await conn.QuerySingleOrDefaultAsync<MirrorComponentUiElement>(sql, new { MirrorComponentId = r.Id });
                r.UiElement = element;
            }
        }

        return result.ToArray();
    }

    public async Task<bool> Update(MirrorComponent component)
    {
        var sql = "update mirrors_components set active = @Active, name = @Name, schedule = @Schedule, tokenId = @TokenId, lastUpdatedTimeUtc = @LastUpdatedTimeUtc where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.ExecuteAsync(sql, component);
        var success = result > 0;
        return success;
    }
}
