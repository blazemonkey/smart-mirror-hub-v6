using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IMirrorRepository
{
    Task<Mirror[]> GetAll();
    Task<Mirror> GetById(int id);
}
