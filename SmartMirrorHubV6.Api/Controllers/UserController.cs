using Microsoft.AspNetCore.Mvc;
using SmartMirrorHubV6.Api.Database;
using SmartMirrorHubV6.Api.Database.Models;

namespace SmartMirrorHubV6.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : BaseController
{
    public UserController(IUnitOfWork unitOfWork) : base(unitOfWork) { }

    [HttpGet(Name = "GetAllUsers")]
    public async Task<User[]> GetAll()
    {
        var users = await UnitOfWork.Users.GetAll();
        return users.ToArray();
    }
}
