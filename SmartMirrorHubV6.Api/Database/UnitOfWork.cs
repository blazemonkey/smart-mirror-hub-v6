using SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

namespace SmartMirrorHubV6.Api.Database;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(IComponentRepository components, IUserRepository users)
    {
        Components = components;
        Users = users;
    }
    public IComponentRepository Components { get; }
    public IUserRepository Users { get; }
}
