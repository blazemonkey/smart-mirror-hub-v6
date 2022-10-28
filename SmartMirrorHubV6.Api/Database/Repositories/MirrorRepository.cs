using Dapper;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class MirrorRepository : BaseRepository, IMirrorRepository
{
    public MirrorRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<Mirror[]> GetAll(bool includeComponents = false)
    {
        var sql = "select * from mirrors";
        using var conn = await OpenConnection();
        var result = await conn.QueryAsync<Mirror>(sql);

        foreach (var r in result)
        {
            if (includeComponents)
            {
                sql = "select * from mirrors_components where mirrorid = @MirrorId";
                var components = await conn.QueryAsync<MirrorComponent>(sql, new { MirrorId = r.Id });
                r.MirrorComponents = components.ToArray();
            }

            sql = "select * from mirrors_voice_devices where mirrorid = @MirrorId";
            var devices = await conn.QueryAsync<MirrorVoiceDevice>(sql, new { MirrorId = r.Id });
            r.VoiceDevices = devices.ToArray();
        }       

        return result.ToArray();
    }

    public async Task<Mirror> GetById(int id, bool includeComponents = false)
    {
        var sql = "select * from mirrors where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<Mirror>(sql, new { Id = id });
        if (result == null)
            return null;

        if (includeComponents)
        {
            sql = "select * from mirrors_components where mirrorid = @MirrorId";
            var components = await conn.QueryAsync<MirrorComponent>(sql, new { MirrorId = id });
            result.MirrorComponents = components.ToArray();
        }

        sql = "select * from mirrors_voice_devices where mirrorid = @MirrorId";
        var devices = await conn.QueryAsync<MirrorVoiceDevice>(sql, new { MirrorId = result.Id });
        result.VoiceDevices = devices.ToArray();

        return result;
    }

    public async Task<Mirror> GetByVoiceDeviceId(string voiceDeviceId, bool includeComponents = false)
    {
        var sql = "select m.* from mirrors m join mirrors_voice_devices where deviceid = @DeviceId";
        using var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<Mirror>(sql, new { DeviceId = voiceDeviceId });
        if (result == null)
            return null;

        if (includeComponents)
        {
            sql = "select * from mirrors_components where mirrorid = @MirrorId";
            var components = await conn.QueryAsync<MirrorComponent>(sql, new { MirrorId = result.Id });
            result.MirrorComponents = components.ToArray();
        }

        sql = "select * from mirrors_voice_devices where mirrorid = @MirrorId";
        var devices = await conn.QueryAsync<MirrorVoiceDevice>(sql, new { MirrorId = result.Id });
        result.VoiceDevices = devices.ToArray();

        return result;
    }
}
