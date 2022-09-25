using Microsoft.AspNetCore.Mvc;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class MirrorController : BaseController
{
    public MirrorController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [HttpGet(Name = "GetAlMirrors")]
    public async Task<Mirror[]> GetAll([FromQuery] bool includeComponents)
    {
        var mirrors = await UnitOfWork.Mirrors.GetAll(includeComponents);
        return mirrors.ToArray();
    }
}
