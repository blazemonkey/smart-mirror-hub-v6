using Dapper;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class MirrorRepository : BaseRepository, IMirrorRepository
{
    public MirrorRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<Mirror[]> GetAll()
    {
        var sql = "select * from mirrors";
        using var conn = await OpenConnection();
        var result = await conn.QueryAsync<Mirror>(sql);
        return result.ToArray();
    }

    public async Task<Mirror> GetById(int id)
    {
        var sql = "select * from mirrors where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<Mirror>(sql, new { Id = id });
        return result;
    }
}
