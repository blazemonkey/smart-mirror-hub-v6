using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IComponentRepository components, IMirrorRepository mirrors, IUserRepository users)
    {
        Components = components;
        Mirrors = mirrors;
        Users = users;
    }
    public IComponentRepository Components { get; }
    public IMirrorRepository Mirrors { get; }
    public IUserRepository Users { get; }
}
