﻿using Dapper;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class ResponseHistoryRepository : BaseRepository, IResponseHistoryRepository
{
    public ResponseHistoryRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<ResponseHistory> GetLatestByMirrorComponentId(int mirrorComponentId)
    {
        var sql = "select * from response_history where mirrorcomponentid = @MirrorComponentId and success = 1 order by dateTimeUtc desc limit 1";
        var conn = await OpenConnection();
        var result = await conn.QuerySingleOrDefaultAsync<ResponseHistory>(sql, new { MirrorComponentId = mirrorComponentId });
        return result;
    }

    public async Task<bool> Insert(ResponseHistory response)
    {
        var sql = "insert into response_history (mirrorComponentId, dateTimeUtc, success, response, timeTaken) values (@MirrorComponentId, @DateTimeUtc, @Success, @Response, @TimeTaken)";
        var conn = await OpenConnection();
        var result = await conn.ExecuteAsync(sql, response);
        var success = result > 0;
        return success;
    }
}