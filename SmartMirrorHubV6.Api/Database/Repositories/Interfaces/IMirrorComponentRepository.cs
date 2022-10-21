using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IMirrorComponentRepository
{
    Task<MirrorComponent[]> GetAllByMirrorId(int mirrorId);
    Task<MirrorComponent[]> GetAllByUserIdAndMirrorName(int userId, string mirrorName, bool includeSettings = false, bool includeUiElement = false);
    Task<MirrorComponent> GetById(int id, bool includeSettings = false, bool includeUiElement = false);
    Task<bool> Update(MirrorComponent component);
}
