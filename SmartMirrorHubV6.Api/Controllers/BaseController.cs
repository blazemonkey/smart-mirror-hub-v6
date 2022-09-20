using SmartMirrorHubV6.Api.Database;

namespace SmartMirrorHubV6.Api.Controllers;

public class BaseController
{
    public IUnitOfWork UnitOfWork { get; private set; }
    public BaseController(IUnitOfWork unitOfWork)
    {
        UnitOfWork = unitOfWork;
    }
}

