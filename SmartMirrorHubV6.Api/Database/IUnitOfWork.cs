using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database;

public interface IUnitOfWork
{
    IComponentRepository Components { get; }
    IMirrorRepository Mirrors { get; }
    IUserRepository Users { get; }
}
