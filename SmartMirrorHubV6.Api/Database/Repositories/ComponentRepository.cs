using Dapper;
using MySql.Data.MySqlClient;
using SmartMirrorHubV6.Api.Database.Models;
using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database.Repositories;

public class ComponentRepository : BaseRepository, IComponentRepository
{
    private ComponentSettingRepository _componentSettingRepository;
    public ComponentRepository(IConfiguration configuration) : base(configuration) 
    {
        _componentSettingRepository = new ComponentSettingRepository(configuration);
    }

    public async Task<Component> GetById(int id, bool includeSettings = false)
    {
        var sql = "select * from components where id = @Id";
        MySqlConnection conn = await OpenConnection();

        var result = await conn.QuerySingleOrDefaultAsync<Component>(sql, new { Id = id });
        if (result == null)
            return null;

        if (includeSettings)
            result.Settings = await _componentSettingRepository.GetByComponentId(result.Id, conn);        

        return result;
    }

    public async Task<Component[]> GetAll(bool includeSettings = false, MySqlConnection connection = null)
    {
        var sql = "select * from components";
        MySqlConnection conn;
        if (connection == null)
            conn = await OpenConnection();
        else
            conn = connection;

        var result = await conn.QueryAsync<Component>(sql);
        if (includeSettings)
        {
            foreach (var r in result)
            {
                r.Settings = await _componentSettingRepository.GetByComponentId(r.Id, conn);
            }
        }

        if (connection == null)
            await conn.CloseAsync();

        return result.ToArray();
    }

    public async Task<bool> Insert(Component component, MySqlConnection connection = null)
    {
        var sql = "insert into components (name, description, author, category, `interval`, type, voiceName, hasJs) values (@Name, @Description, @Author, @Category, @Interval, @Type, @VoiceName, @HasJavaScript)";
        MySqlConnection conn;
        if (connection == null)
            conn = await OpenConnection();
        else
            conn = connection;

        var result = await conn.ExecuteAsync(sql, component);
        var success = result > 0;
        if (success == false)
            return false;

        var id = await conn.ExecuteScalarAsync<int>("SELECT LAST_INSERT_ID();");
        foreach (var setting in component.Settings)
        {
            setting.ComponentId = id;
            sql = "insert into components_settings (componentId, name, type, displayName) values (@ComponentId, @Name, @Type, @DisplayName)";
            result = await conn.ExecuteAsync(sql, setting);
            success = result > 0;
            if (success == false)
                return false;
        }

        if (connection == null)
            await conn.CloseAsync();

        return true;
    }

    public async Task<bool> Update(Component component, MySqlConnection connection = null)
    {
        var sql = "update components set name = @Name, description = @Description, category = @Category, `interval` = @Interval, type = @Type, voiceName = @VoiceName, hasJs = @HasJavaScript where id = @Id";
        MySqlConnection conn;
        if (connection == null)
            conn = await OpenConnection();
        else
            conn = connection;

        var result = await conn.ExecuteAsync(sql, component);
        var success = result > 0;
        if (success == false)
            return false;

        var dbSettings = await _componentSettingRepository.GetByComponentId(component.Id, conn);
        foreach (var setting in component.Settings)
        {
            setting.ComponentId = component.Id;
            var exist = dbSettings.FirstOrDefault(x => x.Name == setting.Name);
            if (exist == null)
            {
                sql = "insert into components_settings (componentId, name, type, displayName) values (@ComponentId, @Name, @Type, @DisplayName)";
                result = await conn.ExecuteAsync(sql, setting);
                success = result > 0;
                if (success == false)
                    return false;
            }
            else
            {
                setting.Id = exist.Id;
                sql = "update components_settings set name = @Name, type = @Type, displayName = @DisplayName where id = @Id";
                result = await conn.ExecuteAsync(sql, setting);
                success = result > 0;
                if (success == false)
                    return false;
            }
        }

        foreach (var setting in dbSettings)
        {
            var exist = component.Settings.FirstOrDefault(x => x.Name == setting.Name);
            if (exist != null)
                continue;

            sql = "delete from components_settings where id = @Id";
            result = await conn.ExecuteAsync(sql, new { Id = setting.Id});
            success = result > 0;
            if (success == false)
                return false;
        }

        if (connection == null)
            await conn.CloseAsync();

        return true;    
    }

    public async Task<bool> Delete(int id, MySqlConnection connection = null)
    {
        var sql = "delete from components where id = @Id";
        MySqlConnection conn;
        if (connection == null)
            conn = await OpenConnection();
        else
            conn = connection;

        var result = await conn.ExecuteAsync(sql, new { Id = id });
        var success = result > 0;
        if (success == false)
            return false;

        if (connection == null)
            await conn.CloseAsync();

        return true;
    }

    public async Task<bool> ReplaceAll(Component[] components)
    {
        using var conn = await OpenConnection();
        var dbComponents = await GetAll(false, conn);
        var result = await conn.BeginTransactionAsync();

        foreach (var component in components)
        {
            var exist = dbComponents.FirstOrDefault(x => x.Author == component.Author && x.Name == component.Name);
            if (exist == null)
            {
                var success = await Insert(component, conn);
                if (success == false)
                    await result.RollbackAsync();
            }
            else
            {
                component.Id = exist.Id;
                var success = await Update(component, conn);
                if (success == false)
                    await result.RollbackAsync();
            }
        }

        foreach (var component in dbComponents)
        {
            var exist = components.FirstOrDefault(x => x.Author == component.Author && x.Name == component.Name);
            if (exist != null)
                continue;

            var success = await Delete(component.Id, conn);
            if (success == false)
                await result.RollbackAsync();
        }

        await result.CommitAsync();
        return true;
    }
}
