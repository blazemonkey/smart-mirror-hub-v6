using Dapper;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class TokenRepository : BaseRepository, ITokenRepository
{
    public TokenRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<Token> GetById(int id)
    {
        var sql = "select * from tokens where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<Token>(sql, new { Id = id });
        return result;
    }

    public async Task<bool> Update(Token token)
    {
        var sql = "update tokens set clientId = @ClientId, accessToken = @AccessToken, refreshToken = @RefreshToken, expiresAt = @ExpiresAt, expiresIn = @ExpiresIn where id = @Id";
        using var conn = await OpenConnection();
        var result = await conn.ExecuteAsync(sql, token);
        var success = result > 0;
        return success;
    }
}

