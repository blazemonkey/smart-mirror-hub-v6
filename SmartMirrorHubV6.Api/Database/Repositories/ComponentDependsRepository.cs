using Dapper;
using MySql.Data.MySqlClient;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class ComponentDependsRepository : BaseRepository, IComponentDependsRepository
{
    public ComponentDependsRepository(IConfiguration configuration) : base(configuration) { }

    public async Task<ComponentDepends[]> GetByComponentId(int componentId, MySqlConnection connection = null)
    {
        var sql = "select * from components_depends where componentid = @ComponentId";
        MySqlConnection conn;
        if (connection == null)
            conn = await OpenConnection();
        else
            conn = connection;

        var result = await conn.QueryAsync<ComponentDepends>(sql, new { ComponentId = componentId });
        if (connection == null)
            await conn.CloseAsync();

        return result.ToArray();
    }
}
