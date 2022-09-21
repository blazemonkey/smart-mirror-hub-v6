using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database;

public interface IUnitOfWork
{
    IComponentRepository Components { get; }
    IComponentSettingRepository ComponentSettings { get; }
    IMirrorRepository Mirrors { get; }
    IMirrorComponentRepository MirrorComponents { get; }
    IResponseHistoryRepository ResponseHistory { get; }
    IUserRepository Users { get; }
}
