using MySql.Data.MySqlClient;
using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IComponentSettingRepository
{
    Task<ComponentSetting[]> GetByComponentId(int componentId, MySqlConnection connection = null);
}
