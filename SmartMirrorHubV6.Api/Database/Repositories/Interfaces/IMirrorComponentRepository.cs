using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IMirrorComponentRepository
{
    Task<MirrorComponent[]> GetAllByMirrorId(int mirrorId);
    Task<MirrorComponent> GetById(int id, bool includeSettings = false);
    Task<bool> Update(MirrorComponent component);
}
