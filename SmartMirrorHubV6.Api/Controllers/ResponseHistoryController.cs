using Microsoft.AspNetCore.Mvc;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ResponseHistoryController : BaseController
{
    public ResponseHistoryController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [HttpGet("mirrorComponentId/{mirrorComponentId}", Name = "GetResponseHistoryByMirrorComponentId")]
    public async Task<ResponseHistory[]> GetByMirrorComponentId(int mirrorComponentId)
    {
        var history = await UnitOfWork.ResponseHistory.GetByMirrorComponentId(mirrorComponentId);
        return history.ToArray();
    }

    [HttpDelete(Name = "PruneResponseHistory")]
    public async Task<bool> PruneResponseHistory(DateTime? fromUtc, DateTime toUtc)
    {
        var deleted = await UnitOfWork.ResponseHistory.DeleteByDateRange(fromUtc, toUtc);
        return deleted;
    }
}
