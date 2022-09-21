using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IResponseHistoryRepository
{
    Task<ResponseHistory> GetLatestByMirrorComponentId(int mirrorComponentId);
    Task<bool> Insert(ResponseHistory response);
}
