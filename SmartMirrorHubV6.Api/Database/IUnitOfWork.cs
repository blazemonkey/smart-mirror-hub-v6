using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database;

public interface IUnitOfWork
{
    IComponentRepository Components { get; }
    IUserRepository Users { get; }
}
