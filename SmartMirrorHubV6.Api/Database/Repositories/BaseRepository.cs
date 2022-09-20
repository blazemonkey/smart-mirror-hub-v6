using MySql.Data.MySqlClient;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class BaseRepository
{
    protected IConfiguration Configuration { get; private set; }
    public BaseRepository(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected async Task<MySqlConnection> OpenConnection()
    {
        var connString = GetConnectionString();
        var conn = new MySqlConnection(connString);
        await conn.OpenAsync();
        return conn;
    }
    protected string GetConnectionString()
    {
        var connString = Configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connString))
            throw new Exception("No Connection String found");

        return connString;
    }
}
