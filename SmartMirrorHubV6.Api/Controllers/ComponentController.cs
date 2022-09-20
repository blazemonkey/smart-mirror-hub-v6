using Microsoft.AspNetCore.Mvc;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ComponentController : BaseController
{
    public ComponentController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [HttpGet(Name = "GetAllComponents")]
    public async Task<Component[]> GetAll()
    {
        var components = await UnitOfWork.Components.GetAll(includeSettings: true);
        return components.ToArray();
    }
}
