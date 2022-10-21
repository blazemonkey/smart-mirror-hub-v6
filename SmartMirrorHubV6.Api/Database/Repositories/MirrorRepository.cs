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

        if (includeComponents)
        {
            foreach (var r in result)
            {
                sql = "select * from mirrors_components where mirrorid = @MirrorId";
                var components = await conn.QueryAsync<MirrorComponent>(sql, new { MirrorId = r.Id });
                r.MirrorComponents = components.ToArray();
            }
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
        return result;
    }
}
