using Dapper;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<User[]> GetAll()
    {
        var sql = "select * from users";
        using var conn = await OpenConnection();
        var result = await conn.QueryAsync<User>(sql);
        return result.ToArray();
    }

    public async Task<User> GetById(int id)
    {
        var sql = "select * from users where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        return result;
    }
}
