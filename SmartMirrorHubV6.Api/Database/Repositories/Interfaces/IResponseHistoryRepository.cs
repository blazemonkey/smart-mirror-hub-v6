using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Database.Repositories.Interfaces;

public interface IResponseHistoryRepository
{
    Task<ResponseHistory> GetLatestByMirrorComponentId(int mirrorComponentId);
    Task<ResponseHistory[]> GetByMirrorComponentId(int mirrorComponentId);
    Task<bool> DeleteByDateRange(DateTime? fromUtc, DateTime toUtc);
    Task<bool> Insert(ResponseHistory response);
}
