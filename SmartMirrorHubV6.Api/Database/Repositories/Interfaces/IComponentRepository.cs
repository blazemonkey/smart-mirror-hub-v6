using MySql.Data.MySqlClient;
using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IComponentRepository
{
    Task<Component> GetById(int id, bool includeSettings = false);
    Task<Component[]> GetAll(bool includeSettings = false, MySqlConnection connection = null);
    Task<bool> ReplaceAll(Component[] components);
}
